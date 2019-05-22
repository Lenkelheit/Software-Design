using DataAccess.Entities;
using DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic for request table
    /// </summary>
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        /// <summary>
        /// Gets request by user and apartment ids
        /// </summary>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <param name="apartmentId">
        /// Apartment's id
        /// </param>
        /// <returns>
        /// An instance of <see cref="Request"/>
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual Request GetByValues(int userId, int apartmentId)
        {
            ContextCheck();

            return dbSet.FirstOrDefault(request => request.Apartment.Id == apartmentId && request.Resident.Id == userId);
        }

        /// <summary>
        /// Gets requests with some information
        /// </summary>
        /// <param name="managerId">
        /// Manager's id
        /// </param>
        /// <param name="amount">
        /// Records amount to select
        /// </param>
        /// <param name="page">
        /// Records offset
        /// </param>
        /// <returns>
        /// Collection of requests
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<Request> GetShortInfo(int managerId, int page, int amount)
        {
            ContextCheck();

            IQueryable<Request> query = dbSet.Where(r => r.Resident.Manager.Id == managerId)
                                             .Include(request => request.Resident)
                                             .Include(request => request.Apartment);

            // [resident id, [payment status, amount]]
            IDictionary<int, Dictionary<Enums.PaymentStatus, int>> residentPaymentStatus = 
                (from bill in dbContext.Set<Bill>()
                where bill.Apartment != null && bill.Renter != null
                group bill by bill.Renter.Id into billGroup
                select new
                {
                    renterId = billGroup.Key,

                    status = billGroup
                            .Where(b => b.Renter.Id == billGroup.Key)
                            .GroupBy(b => b.PaymentStatus)
                            //.ToDictionary(b => b.Key, b => b.Count())
                            .Select(b => new { b.Key, Amount = b.Count() })
                            .ToDictionary(b => b.Key, b => b.Amount)
                })
                .ToDictionary(q => q.renterId, q => q.status);

            return query
                   .Skip((page - 1) * amount)
                   .Take(amount)
                   .AsEnumerable()
                   .Select(request =>
                   {
                       int renterId = request.Resident.Id;

                       // if user has any bills..
                       if (residentPaymentStatus.ContainsKey(renterId))
                       {
                           // .. count how many he paid properly
                           int total = residentPaymentStatus[renterId].Sum(x => x.Value);
                           int positive = residentPaymentStatus[renterId].ContainsKey(Enums.PaymentStatus.Paid) ? residentPaymentStatus[renterId][Enums.PaymentStatus.Paid] : 0;

                           request.PercentagePayedProperly = Percentage(total, positive);
                       }
                       else
                       {
                           // .. sets payment status to 100
                           request.PercentagePayedProperly = 100;
                       }
                       return request;
                   });
        }
        
        private int Percentage(float total, float current)
        {
            if (total == 0) return 100;
            return (int)System.Math.Round(current / total * 100);
        }

    }
}

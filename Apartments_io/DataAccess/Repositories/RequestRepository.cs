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
        public Request GetByValues(int userId, int apartmentId)
        {
            ContextCheck();

            return dbSet.FirstOrDefault(request => request.Apartment.Id == apartmentId && request.Resident.Id == userId);
        }

        /// <summary>
        /// Gets requests with some information
        /// </summary>
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
        public IEnumerable<Request> GetShortInfo(int page, int amount)
        {
            ContextCheck();

            IQueryable<Request> query = dbSet.Include(request => request.Resident)
                                             .Include(request => request.Apartment);

            IDictionary<int, Dictionary<Enums.PaymentStatus, int>> v = 
                (from bill in dbContext.Set<Bill>().Include(b => b.Renter)
                group bill by bill.Renter.Id into billGroup
                select new
                {
                    renterId = billGroup.Key,

                    status = new Dictionary<Enums.PaymentStatus, int>()
                    /* billGroup
                            .Where(b => b.Renter != null)
                            .Where(b => b.Renter.Id == billGroup.Key)
                            .GroupBy(b => b.PaymentStatus)
                            .ToDictionary(s => s.Key, s => s.Count())*/
                })
                .ToDictionary(q => q.renterId, q => q.status);
            

            return query
                   .Skip((page - 1) * amount)
                   .Take(amount)
                   .AsEnumerable()
                   .Select(request =>
                   {
                       int renterId = request.Resident.Id;
                       
                       int total = v[renterId].Sum(x => x.Value);
                       int positive = v[renterId][Enums.PaymentStatus.Paid];

                       request.PercentagePayedProperly = Percentage(total, positive);
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

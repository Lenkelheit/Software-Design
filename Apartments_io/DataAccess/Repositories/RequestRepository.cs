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
        /// An instance of <see cref="DataAccess.Entities.Request"/>
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

            IQueryable<Request> query = dbSet.Include(nameof(Request.Resident))
                                             .Include(nameof(Request.Apartment));

            // TODO: maybe change this to auto mapper
            return (from request in query
                   select new Request
                   {
                       Id = request.Id,
                       Apartment = new Apartment()
                       {
                           Id = request.Apartment.Id,
                           RentEndDate = request.Apartment.RentEndDate
                       },
                       Resident = new User()
                       {
                           FirstName = request.Resident.FirstName,
                           LastName = request.Resident.LastName
                       }
                   })
                   .Skip((page - 1) * amount)
                   .Take(amount);
        }
    }
}

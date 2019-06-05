using DataAccess.Entities;
using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic for requests
    /// </summary>
    public interface IRequestRepository : IRepository<Request>
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
        Request GetByValues(int userId, int apartmentId);
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
        IEnumerable<Request> GetShortInfo(int managerId, int page, int amount);
    }
}

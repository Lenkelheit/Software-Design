using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic for apartments
    /// </summary>
    public interface IApartmentRepository : IRepository<Entities.Apartment>
    {
        /// <summary>
        /// Gets image relative path by id
        /// </summary>
        /// <param name="id">
        /// Apartment's id
        /// </param>
        /// <returns>
        /// Relative image path
        /// </returns>
        string GetImageById(int id);
        /// <summary>
        /// Return collection of random free apartments
        /// </summary>
        /// <param name="amount">
        /// Amount of items to return
        /// </param>
        /// <returns>
        /// Collection of random apartments
        /// </returns>
        IEnumerable<Entities.Apartment> GetRandom(int amount);
        /// <summary>
        /// Determines if user rent current apartment
        /// </summary>
        /// <param name="apartmentId">
        /// Apartment's id
        /// </param>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <returns>
        /// True if user is renter, otherwise — false
        /// </returns>
        bool IsRenter(int apartmentId, int userId);
        /// <summary>
        /// Determines if users has sent request for current apartments
        /// </summary>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <param name="apartmentsIds">
        /// Apartments' ids to check
        /// </param>
        /// <returns>
        /// Sets with apartments' ids for which user has sent request
        /// </returns>
        ISet<int> HasRequests(int userId, IEnumerable<int> apartmentsIds);
        /// <summary>
        /// Determines if users has sent request for current apartment
        /// </summary>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <param name="apartmentId">
        /// Apartments' id
        /// </param>
        /// <returns>
        /// True if user has sent request for current apartment, otherwise — false
        /// </returns>
        bool HasRequest(int userId, int apartmentId);
        /// <summary>
        /// Gets ids of apartments for current user that will expire in current period
        /// </summary>
        /// <param name="renterId">
        /// Renter's id
        /// </param>
        /// <param name="daysToExpire">
        /// Expire period for apartments
        /// </param>
        /// <returns>
        /// List with ids of apartments that will expire in current period
        /// </returns>
        IEnumerable<int> ExpiredApartment(int renterId, int daysToExpire);
    }
}

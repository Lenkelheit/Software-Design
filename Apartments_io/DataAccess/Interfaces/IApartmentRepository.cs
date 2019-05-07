namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic for apartments
    /// </summary>
    public interface IApartmentRepository : IRepository<Entities.Apartment>
    {
        /// <summary>
        /// Return collection of best apartments
        /// </summary>
        /// <param name="amount">
        /// Amount of items to return
        /// </param>
        /// <returns>
        /// Collection of best apartments
        /// </returns>
        System.Collections.Generic.IEnumerable<Entities.Apartment> GetBest(int amount);
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
    }
}

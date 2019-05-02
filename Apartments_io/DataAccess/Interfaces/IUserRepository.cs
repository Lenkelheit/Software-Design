namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic for user
    /// </summary>
    public interface IUserRepository : IRepository<Entities.User>
    {
        /// <summary>
        /// Gets collections of user by theirs role
        /// </summary>
        /// <param name="role">
        /// User role
        /// </param>
        /// <returns>
        /// A collection of user of certain role
        /// </returns>
        System.Collections.Generic.IEnumerable<Entities.User> GetUserByRole(Enums.Role role);
        /// <summary>
        /// Determines if manager has any resident
        /// </summary>
        /// <param name="manager">
        /// manager to check
        /// </param>
        /// <returns>
        /// True if manager has any residents, otherwise — false
        /// </returns>
        bool DoesManagerHasAnyResident(Entities.User manager);
    }
}

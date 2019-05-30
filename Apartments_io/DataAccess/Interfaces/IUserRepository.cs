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
        /// <summary>
        /// Checks if current email and password are valid
        /// </summary>
        /// <param name="email">
        /// The email that should be checked
        /// </param>
        /// <param name="password">
        /// The password that should be checked
        /// </param>
        /// <returns>
        /// Returns a structure that defines if Email and Password are valid and by them returns this user 
        /// </returns>
        System.Tuple<bool, bool, Entities.User> IsDataValid(string email, string password);
        /// <summary>
        /// Checks if current email is free
        /// </summary>
        /// <param name="email">
        /// Email that should be checked
        /// </param>
        /// <returns>
        /// True if email is free, otherwise false
        /// </returns>
        bool IsEmailFree(string email);
        /// <summary>
        /// Checks is user is last in his role
        /// </summary>
        /// <param name="role">
        /// Role to be checked
        /// </param>
        /// <returns>
        /// True if user is last in his role, otherwise — falses
        /// </returns>
        bool IsLastIn(Enums.Role role);
        /// <summary>
        /// Gets bill statistic for users of current manager
        /// </summary>
        /// <param name="managerId">
        /// Manager's id
        /// </param>
        /// <returns>
        /// Collection of users bills statistic
        /// </returns>
        System.Collections.Generic.IEnumerable<Wrappers.UserStatisticWrapper> GetUserStatistics(int managerId);
    }
}

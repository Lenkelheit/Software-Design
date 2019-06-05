namespace DataAccess.Interfaces
{
    /// <summary>
    /// Provides a way to set data base's context
    /// </summary>
    /// <typeparam name="TClass">
    /// A derivative class type
    /// </typeparam>
    public interface IDbContextSettable<out TClass>
    {
        /// <summary>
        /// Sets data base's context
        /// </summary>
        /// <param name="dbContext">
        /// A data base's context to set
        /// </param>
        /// <returns>
        /// An instance of class that sets data base's context
        /// </returns>
        TClass SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies dbContext);
    }
}

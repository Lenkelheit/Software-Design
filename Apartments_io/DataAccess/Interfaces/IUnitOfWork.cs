namespace DataAccess.Interfaces
{
    /// <summary>
    /// Provides an mechanism for creating repositories
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Returns a repository for current entity
        /// </summary>
        /// <typeparam name="TEntity">
        /// An entity type for which repository should be returned
        /// </typeparam>
        /// <typeparam name="TRepository">
        /// An type of repository
        /// </typeparam>
        /// <returns>
        /// Repository for current entity
        /// </returns>
        TRepository GetRepository<TEntity, TRepository>()
            where TEntity : Entities.EntityBase, new()
            where TRepository : IRepository<TEntity>, IDbContextSettable<IRepository<TEntity>>, new();

        /// <summary>
        /// Updates entity in collection
        /// </summary>
        /// <param name="entityToUpdate">
        /// Entity to update
        /// </param>       
        /// <typeparam name="TEntity">
        /// An entity type to update
        /// </typeparam>
        void Update<TEntity>(TEntity entityToUpdate) where TEntity : Entities.EntityBase;
        /// <summary>
        /// Saves all changes made in context 
        /// </summary>
        /// <returns>
        /// The number of state entries written
        /// </returns>
        int Save();
        /// <summary>
        /// Asynchronously saves all changes made in context 
        /// </summary>
        /// <returns>
        /// The number of written state entries 
        /// </returns>
        System.Threading.Tasks.Task<int> SaveAsync();
    }
}

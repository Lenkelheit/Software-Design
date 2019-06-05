using DataAccess.Entities;
using DataAccess.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    /// <summary>
    /// Contains all the repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork, System.IDisposable
    {
        // FIELDS
        private readonly DbContext dataBaseContext;
        private readonly IDictionary<System.Type, object> repositoriesFactory;

        private bool isDisposed; // To detect redundant calls

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="UnitOfWork"/> with current Data Base Context
        /// </summary>
        /// <param name="dbContext">
        /// An instance of class that derivative from <see cref="DbContext"/>
        /// </param>
        public UnitOfWork(DbContext dbContext)
        {
            this.dataBaseContext = dbContext;
            this.repositoriesFactory = new Dictionary<System.Type, object>();
            this.isDisposed = false;
        }
        /// <summary>
        /// Disposes DataBase context
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// Determines if managed resources should be disposed
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    dataBaseContext.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below
                isDisposed = true;
            }
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        // METHODS
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
        public TRepository GetRepository<TEntity, TRepository>()
            where TEntity : EntityBase, new()
            where TRepository : IRepository<TEntity>, IDbContextSettable<IRepository<TEntity>>, new ()
        {
            System.Type key = typeof(TEntity);

            // add repo, lazy loading
            if (!repositoriesFactory.ContainsKey(key))
            {
                repositoriesFactory.Add(key, new TRepository().SetDbContext(dataBaseContext));
            }

            // return repository
            return (TRepository)repositoriesFactory[key];
        }

        /// <summary>
        /// Updates database
        /// </summary>
        /// <param name="entityToUpdate">
        /// Entity to update
        /// </param>
        /// <typeparam name="TEntity">
        /// An entity type to update
        /// </typeparam>
        public virtual void Update<TEntity>(TEntity entityToUpdate) where TEntity : EntityBase
        {
            if (dataBaseContext.Entry(entityToUpdate).State == EntityState.Detached)
            {
                dataBaseContext.Set<TEntity>().Attach(entityToUpdate);
            }
            dataBaseContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. <para/>
        /// A concurrency violation occurs when an unexpected number of rows are affected during save. <para/>
        /// This is usually because the data in the database has been modified since it was loaded into memory. <para/>
        /// </exception>
        public int Save()
        {
            return dataBaseContext.SaveChanges();
        }
        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation. <para/>
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        /// <exception cref="DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="DbUpdateConcurrencyException">
        /// A concurrency violation is encountered while saving to the database. <para/>
        /// A concurrency violation occurs when an unexpected number of rows are affected during save. <para/>
        /// This is usually because the data in the database has been modified since it was loaded into memory. <para/>
        /// </exception>
        public Task<int> SaveAsync()
        {
            return dataBaseContext.SaveChangesAsync();
        }
    }
}

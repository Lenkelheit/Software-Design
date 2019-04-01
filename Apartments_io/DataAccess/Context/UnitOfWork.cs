﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Context
{
    /// <summary>
    /// Contains all the repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        // FIELDS
        private readonly DataBaseContext dataBaseContext;
        private readonly IDictionary<System.Type, object> repositoriesFactory;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="UnitOfWork"/>
        /// </summary>
        public UnitOfWork()
            : this(DataBaseContext.Instance) { }
        /// <summary>
        /// Initializes a new instance of <see cref="UnitOfWork"/> with current Data Base Context
        /// </summary>
        /// <param name="dbContext">
        /// An instance of <see cref="DataBaseContext"/>
        /// </param>
        public UnitOfWork(DataBaseContext dbContext)
        {
            this.dataBaseContext = dbContext;
            this.repositoriesFactory = new Dictionary<System.Type, object>();
        }
        /// <summary>
        /// Disposes DataBase context
        /// </summary>
        ~UnitOfWork()
        {
            dataBaseContext.Dispose();
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
            where TRepository : IRepository<TEntity>, new ()
        {
            System.Type key = typeof(TEntity);

            // add repo, lazy loading
            if (!repositoriesFactory.ContainsKey(key))
            {
                repositoriesFactory.Add(key, new TRepository());
            }

            // return repository
            return (TRepository)repositoriesFactory[key];
        }
        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
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
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
        /// An error is encountered while saving to the database.
        /// </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
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

﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Reprents interface for classes which will proxy behind data access and buisness logic
    /// </summary>
    /// <typeparam name="TEntity">
    /// Entity type
    /// </typeparam>
    public interface IRepository<TEntity> where TEntity : Entities.EntityBase, new()
    {
        /// <summary>
        /// Counts amount of entities
        /// </summary>
        /// <returns>
        /// Count of entities
        /// </returns>
        int Count();
        /// <summary>
        /// Counts records which satisfy the condition
        /// </summary>
        /// <param name="predicate">
        /// The condition by which record should be count
        /// </param>
        /// <returns>
        /// Returns the amount of records which satisfy the condition
        /// </returns>
        int Count(Func<TEntity, bool> predicate);
        /// <summary>
        /// Returns data
        /// </summary>
        /// <param name="filter">
        /// Filter for data
        /// </param>
        /// <param name="orderBy">
        /// The order of the received items
        /// </param>
        /// <param name="includeProperties">
        /// Included properties
        /// </param>
        /// <returns>
        /// Queried entities collection
        /// </returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                 string includeProperties = "");
        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">
        /// Entity's id
        /// </param>
        /// <returns>
        /// Finded entity
        /// </returns>
        TEntity Get(object id);
        /// <summary>
        /// Inserts data in collection
        /// </summary>
        /// <param name="entity">
        /// Inserted entity
        /// </param>
        void Insert(TEntity entity);
        /// <summary>
        /// Deletes object by id
        /// </summary>
        /// <param name="id">
        /// Object's id
        /// </param>
        void Delete(object id);
        /// <summary>
        /// Deletes preset entity
        /// </summary>
        /// <param name="entityToDelete">
        /// Entity to delete
        /// </param>
        void Delete(TEntity entityToDelete);
        /// <summary>
        /// Updates entity in collection
        /// </summary>
        /// <param name="entityToUpdate">
        /// Entity to update
        /// </param>
        void Update(TEntity entityToUpdate);
    }
}

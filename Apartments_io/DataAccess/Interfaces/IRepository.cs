using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic
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
        int Count(Expression<Func<TEntity, bool>> predicate);
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
        /// <param name="amount">
        /// Records amount to select
        /// </param>
        /// <param name="page">
        /// Records offset
        /// </param>
        /// <returns>
        /// Queried entities collection
        /// </returns>
        System.Collections.Generic.IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                             string includeProperties = "",
                                                             int? page = null, int? amount = null);
        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">
        /// Entity's id
        /// </param>
        /// <param name="includeProperties">
        /// Included properties
        /// </param>
        /// <returns>
        /// Finded entity
        /// </returns>
        TEntity Get(int id, string includeProperties = "");
        /// <summary>
        /// Returns entity by id
        /// </summary>
        /// <param name="id">
        /// Entity's id
        /// </param>
        /// <param name="includeProperties">
        /// Included properties
        /// </param>
        /// <returns>
        /// Finded entity
        /// </returns>
        System.Threading.Tasks.Task<TEntity> GetAsync(int id, string includeProperties = "");
        /// <summary>
        /// Inserts data in collection
        /// </summary>
        /// <param name="entity">
        /// Inserted entity
        /// </param>
        void Insert(TEntity entity);
        /// <summary>
        /// Inserts data in data base
        /// </summary>
        /// <param name="entity">
        /// Inserted entity
        /// </param>
        /// <returns>
        /// Inserted entity
        /// </returns>
        System.Threading.Tasks.Task<TEntity> InsertAsync(TEntity entity);
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
        /// Deletes entities by predicate
        /// </summary>
        /// <param name="predicate">
        /// Predicate by which entities will be deleted
        /// </param>
        void Delete(Expression<Func<TEntity, bool>> predicate);
    }
}

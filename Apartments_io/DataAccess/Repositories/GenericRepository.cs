using System;
using System.Linq;
using System.Linq.Expressions;

using DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic
    /// </summary>
    /// <typeparam name="TEntity">
    /// Type of entity
    /// </typeparam>
    public class GenericRepository<TEntity> : IDbContextSettable<GenericRepository<TEntity>>, IRepository<TEntity> where TEntity: Entities.EntityBase, new()
    {
        // FIELDS
        private Context.DataBaseContext dbContext;
        private DbSet<TEntity> dbSet;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes new instance of <see cref="GenericRepository{TEntity}"/>
        /// </summary>
        public GenericRepository()
        {
            this.dbContext = null;
            this.dbSet = null;
        }
        /// <summary>
        /// Initializes new instance of <see cref="GenericRepository{TEntity}"/> with passed DataBase context
        /// </summary>
        /// <param name="dbContext">
        /// An instance of <see cref="Context.DataBaseContext"/>
        /// </param>
        public GenericRepository(Context.DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }
        /// <summary>
        /// Sets context for this repository
        /// </summary>
        /// <param name="dbContext">
        /// An context to set
        /// </param>
        /// <returns>
        /// An instance of <see cref="GenericRepository{TEntity}"/>
        /// </returns>
        public virtual GenericRepository<TEntity> SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies dbContext)
        {
            if (dbContext is Context.DataBaseContext dataBaseContext)
            {
                this.dbContext = dataBaseContext;
                this.dbSet = dataBaseContext.Set<TEntity>();
            }
            return this;
        }
        // METHODS
        /// <summary>
        /// Counts records in data set
        /// </summary>
        /// <returns>
        /// Count of entities
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual int Count()
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");

            return dbSet.Count();
        }
        /// <summary>
        /// Counts records in data set which satisfy the condition
        /// </summary>
        /// <param name="predicate">
        /// The condition by which record should be count
        /// </param>
        /// <returns>
        /// Returns the amount of records in data set which satisfy the condition
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when passed <paramref name="predicate"/> is null
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual int Count(Func<TEntity, bool> predicate)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");

            return dbSet.Count(predicate);
        }
        /// <summary>
        /// Deletes object by id
        /// </summary>
        /// <param name="id">
        /// Object's id
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws when passed <paramref name="id"/> is null
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Throws when there is no records with such id
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual void Delete(object id)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");

            // find
            if (id == null) throw new ArgumentNullException(nameof(id));
            TEntity entityToDelete = dbSet.Find(id);

            // delete finded
            if (entityToDelete == null) throw new InvalidOperationException("There is no records with such id");
            Delete(entityToDelete);
        }
        /// <summary>
        /// Deletes preset entity
        /// </summary>
        /// <param name="entityToDelete">
        /// Entity to delete
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Throws when passed <paramref name="entityToDelete"/> is null
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");
            if (entityToDelete == null) throw new ArgumentNullException(nameof(entityToDelete));

            if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        /// <summary>
        /// Gets data from data base
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
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                string includeProperties = "")
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");

            // filter
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // include properties
            foreach (string includeProperty in includeProperties.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            // ordering
            if (orderBy != null) return orderBy(query);
            return query;
        }

        /// <summary>
        /// Gets entity by id
        /// </summary>
        /// <param name="id">
        /// Entity's id
        /// </param>
        /// <returns>
        /// Finded entity or null
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual TEntity Get(object id)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");
            return dbSet.Find(id);
        }
        /// <summary>
        /// Inserts data in data base
        /// </summary>
        /// <param name="entity">
        /// Inserted entity
        /// </param>
        public virtual void Insert(TEntity entity)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");
            dbSet.Add(entity);
        }
        /// <summary>
        /// Updates database
        /// </summary>
        /// <param name="entityToUpdate">
        /// Entity to update
        /// </param>
        /// <exception cref="NullReferenceException">
        /// Throws when context for this repository is not setted<para/>
        /// Try to call <see cref="SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual void Update(TEntity entityToUpdate)
        {
            if (dbSet == null) throw new NullReferenceException($"Data context is not setted. Please call {nameof(SetDbContext)}");
            dbSet.Attach(entityToUpdate);
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

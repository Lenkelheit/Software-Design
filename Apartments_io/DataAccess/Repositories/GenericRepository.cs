using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : Interfaces.IRepository<TEntity> where TEntity: Entities.EntityBase, new()
    {
        // FIELDS
        private Context.DataBaseContext dbContext;
        private Microsoft.EntityFrameworkCore.DbSet<TEntity> dbSet;

        // CONSTRUCTORS
        public GenericRepository(Context.DataBaseContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        // METHODS
        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Count(Func<TEntity, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entityToDelete)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public TEntity Get(object id)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}

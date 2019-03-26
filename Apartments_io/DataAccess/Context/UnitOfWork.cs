using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Context
{
    public class UnitOfWork : Interfaces.IUnitOfWork
    {
        public TRepository GetRepository<TEntity, TRepository>()
            where TEntity : EntityBase, new()
            where TRepository : IRepository<TEntity>
        {
            throw new System.NotImplementedException();
        }

        public int Save()
        {
            throw new System.NotImplementedException();
        }
    }
}

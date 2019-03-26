namespace DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        TRepository GetRepository<TEntity, TRepository>()
            where TEntity : Entities.EntityBase, new()
            where TRepository : IRepository<TEntity>;

        int Save();
    }
}

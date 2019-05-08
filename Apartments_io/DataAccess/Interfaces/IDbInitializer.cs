namespace DataAccess.Interfaces
{
    internal interface IDbInitializer
    {
        void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder);
    }
}

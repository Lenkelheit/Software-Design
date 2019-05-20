namespace DataAccess.Interfaces
{
    /// <summary>
    /// Provides interface to initialize DataBase
    /// </summary>
    public interface IDbInitializer
    {
        /// <summary>
        /// Initialize data base with data
        /// </summary>
        /// <param name="modelBuilder">
        /// An instance of <see cref="Microsoft.EntityFrameworkCore.ModelBuilder"/>
        /// </param>
        void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder);
    }
}

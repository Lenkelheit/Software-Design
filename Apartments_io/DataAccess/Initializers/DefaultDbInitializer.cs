using DataAccess.Entities;

namespace DataAccess.Initializers
{
    internal class DefaultDbInitializer : Interfaces.IDbInitializer
    {
        public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            AddUsers(modelBuilder);            
        }
        private void AddUsers(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            User administrator = new User()
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Password = "1111",
                Role = Enums.Role.Administrator,
            };

            User manager = new User()
            {
                Id = 2,
                FirstName = "Manager",
                LastName = "Manager",
                Email = "manager@gmail.com",
                Password = "1111",
                Role = Enums.Role.Manager,
            };

            modelBuilder.Entity<User>().HasData(administrator, manager);
        }
    }
}

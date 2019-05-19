using DataAccess.Entities;

namespace DataAccess.Tests.TestDataInitializers
{
    class UserTestDataInitializer : Interfaces.IDbInitializer
    {
        // PROPERTIES
        public int UsersCount => 7;
        public int AdministratorCount => 1;
        public int ManagersCount = 2;
        public int ResidentCount => 4;

        public int ManagerIdWithRenters = 2;
        public int ManagerIdWithoutRenters = 3;


        // METHODS
        public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            #region Users
            User user1 = new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@mail.com", Password = "1111", Role = Enums.Role.Administrator };

            User user2 = new User { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@mail.com", Password = "1111", Role = Enums.Role.Manager };
            User user3 = new User { Id = 3, FirstName = "Kayson", LastName = "Swim", Email = "kayson.swim@mail.com", Password = "1111", Role = Enums.Role.Manager }; // no renters

            User user4 = new User { Id = 4, FirstName = "Clementine", LastName = "Oil", Email = "clementine.oil@mail.com", Password = "1111", Role = Enums.Role.Resident };
            User user5 = new User { Id = 5, FirstName = "Beverley", LastName = "Boo", Email = "beverley.boo@mail.com", Password = "1111", Role = Enums.Role.Resident };
            User user6 = new User { Id = 6, FirstName = "Harold", LastName = "Man", Email = "harold.man@mail.com", Password = "1111", Role = Enums.Role.Resident };
            User user7 = new User { Id = 7, FirstName = "Abigail", LastName = "Spencer", Email = "abigail.spencer@mail.com", Password = "1111", Role = Enums.Role.Resident };

            user4.ManagerId = user2.Id;
            user5.ManagerId = user2.Id;
            #endregion
            
            #region Seed
            modelBuilder.Entity<User>().HasData(user1, user2, user3, user4, user5, user6, user7);
            #endregion
        }
    }
}

using DataAccess.Entities;

namespace DataAccess.Tests.TestDataInitializers
{
    class ApartmentTestDataInitializer : Interfaces.IDbInitializer
    {
        // PROPERTIES
        public int ApartmentCount => 5;

        public int ApartmentFor100Count => 2;
        public int ApartmentFor200Count => 3;

        // METHODS
        public void Seed(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            #region Apartments
            Apartment apartment1 = new Apartment { Id = 1, MainPhoto = "photo1.png", Name = "A1", Description = "A1", Price = 100 };
            Apartment apartment2 = new Apartment { Id = 2, MainPhoto = "photo2.png", Name = "A2", Description = "A2", Price = 100 };

            Apartment apartment3 = new Apartment { Id = 3, MainPhoto = "photo3.png", Name = "A3", Description = "A3", Price = 200 };
            Apartment apartment4 = new Apartment { Id = 4, MainPhoto = "photo4.png", Name = "A4", Description = "A4", Price = 200 };
            Apartment apartment5 = new Apartment { Id = 5, MainPhoto = "photo5.png", Name = "A5", Description = "A5", Price = 200 };
            #endregion

            #region Seed
            modelBuilder.Entity<Apartment>().HasData(apartment1, apartment2, apartment3, apartment4, apartment5);
            #endregion
        }
    }
}

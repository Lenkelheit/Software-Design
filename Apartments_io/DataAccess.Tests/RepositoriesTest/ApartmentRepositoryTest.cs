using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.TestDataInitializers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Xunit;

namespace DataAccess.Tests.RepositoriesTest
{
    public class ApartmentRepositoryTest : IDisposable
    {
        // FIELDS
        DataBaseContext testContext;
        ApartmentTestDataInitializer testData;

        // CONSTRUCTORS
        public ApartmentRepositoryTest()
        {
            testData = new ApartmentTestDataInitializer();
            testContext = new DataBaseContext(CreateNewContextOptions(), testData);
        }

        private DbContextOptions<DataBaseContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            return new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;
        }
        public void Dispose()
        {
            testContext.Dispose();
        }

        // PROPERTIES
        ApartmentTestDataInitializer TestData => testData;

        // TEST
        #region COUNT
        [Fact]
        public void CountTest()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            int expectedApartmentCount = TestData.ApartmentCount;
            int expectedApartmentFor100Count = TestData.ApartmentFor100Count;
            int expectedApartmentFor200Count = TestData.ApartmentFor200Count;

            // Act
            int actualApartmentCount = apartmentRepository.Count();
            int actualApartmentFor100Count = apartmentRepository.Count(a => a.Price == 100);
            int actualApartmentFor200Count = apartmentRepository.Count(a => a.Price == 200);

            // Assert
            Assert.Equal(expectedApartmentCount,       actualApartmentCount);
            Assert.Equal(expectedApartmentFor100Count, actualApartmentFor100Count);
            Assert.Equal(expectedApartmentFor200Count, actualApartmentFor200Count);
        }
        #endregion
        #region GET
        [Fact]
        public void GetById()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            Apartment expectedEntity = testContext.Apartments.First();
            int entityId = expectedEntity.Id;

            // Act
            Apartment actualEntity = apartmentRepository.Get(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            Apartment expectedEntity = testContext.Apartments.First();
            int entityId = expectedEntity.Id;

            // Act
            Apartment actualEntity = await apartmentRepository.GetAsync(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public void GetByPredicate()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            int expectedEntityCount = TestData.ApartmentFor100Count;
            Expression<Func<Apartment, bool>> predicateToGet = a => a.Price == 100;
            IEnumerable<Apartment> expectedEntities = testContext.Apartments.Where(predicateToGet);

            // Act
            IEnumerable<Apartment> actualEntities = apartmentRepository.Get(predicateToGet);
            int actualEntitiesCount = actualEntities.Count();

            // Assert
            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(expectedEntityCount, actualEntitiesCount);
        }

        #endregion
        #region DELETE
        [Fact]
        public void DeleteByIdTest()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            Apartment entityToDelete = testContext.Apartments.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.ApartmentCount;
            int expectedCountAfterDelete = TestData.ApartmentCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Apartments.Count();
            apartmentRepository.Delete(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Apartments.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Apartments);
        }
        [Fact]
        public void DeleteByObjectTest()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            Apartment entityToDelete = testContext.Apartments.First();
            int expectedCountBeforeDelete = TestData.ApartmentCount;
            int expectedCountAfterDelete = TestData.ApartmentCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Apartments.Count();
            apartmentRepository.Delete(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Apartments.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Apartments);
        }

        [Fact]
        public void DeleteByPredicateTest()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            Expression<Func<Apartment, bool>> predicateToDelete = a => a.Price == 100;
            int expectedCountBeforeDelete = TestData.ApartmentCount;
            int expectedCountAfterDelete = TestData.ApartmentCount - testData.ApartmentFor100Count;

            // Act
            int actualCountBeforeDelete = testContext.Apartments.Count();
            apartmentRepository.Delete(predicateToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Apartments.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Apartments, a => a.Price == 100);
        }
        #endregion
        #region INSERT
        [Fact]
        public void Insert()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.ApartmentCount;
            int expectedEntitiesAfterInsertCount = TestData.ApartmentCount + 1;

            Apartment entityToInsert = new Apartment { Id = 100, MainPhoto = "photo.png", Name = "A100", Description = "A100", Price = 100 };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Apartments.Count();
            apartmentRepository.Insert(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Apartments.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Apartments);
        }
        [Fact]
        public async Task InsertAsync()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.ApartmentCount;
            int expectedEntitiesAfterInsertCount = TestData.ApartmentCount + 1;

            Apartment entityToInsert = new Apartment { Id = 100, MainPhoto = "photo.png", Name = "A100", Description = "A100", Price = 100 };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Apartments.Count();
            await apartmentRepository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Apartments.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Apartments);
        }
        #endregion
        [Fact]
        public void GetImageByIdTest()
        {
            // Arrange
            ApartmentRepository apartmentRepository = new ApartmentRepository();
            apartmentRepository.SetDbContext(testContext);

            int apartmentId = 2;
            string expectedImage = "photo2.png";

            // Act
            string actualImage = apartmentRepository.GetImageById(apartmentId);

            // Assert
            Assert.Equal(expectedImage, actualImage);
        }

    }
}

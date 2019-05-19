using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.TestDataInitializers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace DataAccess.Tests.RepositoriesTest
{
    public class BillRepositoryTest : IDisposable
    {
        // FIELDS
        DataBaseContext testContext;
        BillTestDataInitializers testData;

        // CONSTRUCTORS
        public BillRepositoryTest()
        {
            testData = new BillTestDataInitializers();
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
        BillTestDataInitializers TestData => testData;

        // TEST
        #region COUNT
        [Fact]
        public void CountTest()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            int expectedBillsCount                  = TestData.BillsCount;
            int expectedPaidBillsCount              = TestData.PaidBillsCount;
            int expectedPaidWithDelayBillsCount     = TestData.PaidWithDelayBillsCount;
            int expectedWaitingForPaymentBillsCount = TestData.WaitingForPaymentBillsCount;
            int expectedOverdueBillsCount           = TestData.OverdueBillsCount;

            // Act
            int actualBillsCount                    = billRepository.Count();
            int actualPaidBillsCount                = billRepository.Count(b => b.PaymentStatus == Enums.PaymentStatus.Paid);
            int actualPaidWithDelayBillsCount       = billRepository.Count(b => b.PaymentStatus == Enums.PaymentStatus.PaidWithDelay);
            int actualWaitingForPaymentBillsCount   = billRepository.Count(b => b.PaymentStatus == Enums.PaymentStatus.WaitingForPayment);
            int actualOverdueBillsCount             = billRepository.Count(b => b.PaymentStatus == Enums.PaymentStatus.Overdue);

            // Assert
            Assert.Equal(expectedBillsCount                 , actualBillsCount                 );
            Assert.Equal(expectedPaidBillsCount             , actualPaidBillsCount             );
            Assert.Equal(expectedPaidWithDelayBillsCount    , actualPaidWithDelayBillsCount    );
            Assert.Equal(expectedWaitingForPaymentBillsCount, actualWaitingForPaymentBillsCount);
            Assert.Equal(expectedOverdueBillsCount          , actualOverdueBillsCount          );
        }
        #endregion
        #region GET
        [Fact]
        public void GetById()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            Bill expectedEntity = testContext.Bills.First();
            int entityId = expectedEntity.Id;

            // Act
            Bill actualEntity = billRepository.Get(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            Bill expectedEntity = testContext.Bills.First();
            int entityId = expectedEntity.Id;

            // Act
            Bill actualEntity = await billRepository.GetAsync(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public void GetByPredicate()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            int expectedEntityCount = TestData.WaitingForPaymentBillsCount;
            Expression<Func<Bill, bool>> predicateToGet = b => b.PaymentStatus == Enums.PaymentStatus.WaitingForPayment;
            IEnumerable<Bill> expectedEntities = testContext.Bills.Where(predicateToGet);

            // Act
            IEnumerable<Bill> actualEntities = billRepository.Get(predicateToGet);
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
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            Bill entityToDelete = testContext.Bills.First();
            int idToDelete = entityToDelete.Id;
            int expectedCountBeforeDelete = TestData.BillsCount;
            int expectedCountAfterDelete = TestData.BillsCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Bills.Count();
            billRepository.Delete(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Bills.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Bills);
        }
        [Fact]
        public void DeleteByObjectTest()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            Bill entityToDelete = testContext.Bills.First();
            int expectedCountBeforeDelete = TestData.BillsCount;
            int expectedCountAfterDelete = TestData.BillsCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Bills.Count();
            billRepository.Delete(entityToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Bills.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(entityToDelete, testContext.Bills);
        }

        [Fact]
        public void DeleteByPredicateTest()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            Expression<Func<Bill, bool>> predicateToDelete = b => b.PaymentStatus == Enums.PaymentStatus.WaitingForPayment;
            int expectedCountBeforeDelete = TestData.BillsCount;
            int expectedCountAfterDelete = TestData.BillsCount - TestData.WaitingForPaymentBillsCount;

            // Act
            int actualCountBeforeDelete = testContext.Bills.Count();
            billRepository.Delete(predicateToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Bills.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Bills, b => b.PaymentStatus == Enums.PaymentStatus.WaitingForPayment);
        }
        #endregion
        #region INSERT
        [Fact]
        public void Insert()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.BillsCount;
            int expectedEntitiesAfterInsertCount = TestData.BillsCount + 1;

            Bill entityToInsert = new Bill { Id = 100, PaymentStatus = Enums.PaymentStatus.WaitingForPayment };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Bills.Count();
            billRepository.Insert(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Bills.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Bills);
        }
        [Fact]
        public async Task InsertAsync()
        {
            // Arrange
            BillRepository billRepository = new BillRepository();
            billRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.BillsCount;
            int expectedEntitiesAfterInsertCount = TestData.BillsCount + 1;

            Bill entityToInsert = new Bill { Id = 100, PaymentStatus = Enums.PaymentStatus.WaitingForPayment };

            // Act
            int actualEntitiesBeforeInsertCount = testContext.Bills.Count();
            await billRepository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualEntitiesAfterInsertCount = testContext.Bills.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualEntitiesBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualEntitiesAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Bills);
        }
        #endregion

    }
}

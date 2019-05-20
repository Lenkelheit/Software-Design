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
    public class NotificationRepositoryTest : IDisposable
    {
        // FIELDS
        DataBaseContext testContext;
        NotificationTestDataInitializer testData;

        // CONSTRUCTORS
        public NotificationRepositoryTest()
        {
            testData = new NotificationTestDataInitializer();
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
        NotificationTestDataInitializer TestData => testData;

        // TEST
        #region COUNT
        [Fact]
        public void CountTest()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            int expectedNotificationCount = TestData.NotificationsCount;
            int expectedInfoNotificationCount = TestData.InfoNotificationsCount;
            int expectedSuccessNotificationCount = TestData.SuccessNotificationsCount;
            int expectedWarningNotificationCount = TestData.WarningNotificationsCount;
            int expectedDangerNotificationCount = TestData.DangerNotificationsCount;

            // Act
            int actualNotificationCount = notificationRepository.Count();
            int actualInfoNotificationCount = notificationRepository.Count(n => n.EmergencyLevel == Enums.EmergencyLevel.Info);
            int actualSuccessNotificationCount = notificationRepository.Count(n => n.EmergencyLevel == Enums.EmergencyLevel.Success);
            int actualWarningNotificationCount = notificationRepository.Count(n => n.EmergencyLevel == Enums.EmergencyLevel.Warning);
            int actualDangerNotificationCount = notificationRepository.Count(n => n.EmergencyLevel == Enums.EmergencyLevel.Danger);

            // Assert
            Assert.Equal(expectedNotificationCount, actualNotificationCount);
            Assert.Equal(expectedInfoNotificationCount, actualInfoNotificationCount);
            Assert.Equal(expectedSuccessNotificationCount, actualSuccessNotificationCount);
            Assert.Equal(expectedWarningNotificationCount, actualWarningNotificationCount);
            Assert.Equal(expectedDangerNotificationCount, actualDangerNotificationCount);
        }
        #endregion
        #region GET
        [Fact]
        public void GetById()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            Notification expectedEntity = testContext.Notifications.First();
            int entityId = expectedEntity.Id;

            // Act
            Notification actualEntity = notificationRepository.Get(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            Notification expectedEntity = testContext.Notifications.First();
            int entityId = expectedEntity.Id;

            // Act
            Notification actualEntity = await notificationRepository.GetAsync(entityId);

            // Assert
            Assert.Equal(expectedEntity, actualEntity);
        }
        [Fact]
        public void GetByPredicate()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            int expectedEntityCount = TestData.InfoNotificationsCount;
            Expression<Func<Notification, bool>> predicateToGet = n => n.EmergencyLevel == Enums.EmergencyLevel.Info;
            IEnumerable<Notification> expectedEntities = testContext.Notifications.Where(predicateToGet);

            // Act
            IEnumerable<Notification> actualEntities = notificationRepository.Get(predicateToGet);
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
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            Notification notificationToDelete = testContext.Notifications.First();
            int idToDelete = notificationToDelete.Id;
            int expectedCountBeforeDelete = TestData.NotificationsCount;
            int expectedCountAfterDelete = TestData.NotificationsCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Notifications.Count();
            notificationRepository.Delete(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Notifications.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
            Assert.DoesNotContain(notificationToDelete, testContext.Notifications);
        }
        [Fact]
        public void DeleteByObjectTest()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            Notification notificationToDelete = testContext.Notifications.First();
            int expectedCountBeforeDelete = TestData.NotificationsCount;
            int expectedCountAfterDelete = TestData.NotificationsCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Notifications.Count();
            notificationRepository.Delete(notificationToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Notifications.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(notificationToDelete, testContext.Notifications);
        }

        [Fact]
        public void DeleteByPredicateTest()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            Expression<Func<Notification, bool>> predicateToDelete = n => n.EmergencyLevel == Enums.EmergencyLevel.Info;
            int expectedCountBeforeDelete = TestData.NotificationsCount;
            int expectedCountAfterDelete = TestData.NotificationsCount - testData.InfoNotificationsCount;

            // Act
            int actualCountBeforeDelete = testContext.Notifications.Count();
            notificationRepository.Delete(predicateToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Notifications.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Notifications, n => n.EmergencyLevel == Enums.EmergencyLevel.Info);
        }
        #endregion
        #region INSERT
        [Fact]
        public void Insert()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.NotificationsCount;
            int expectedEntitiesAfterInsertCount = TestData.NotificationsCount + 1;

            Notification entityToInsert = new Notification { Id = 100, EmergencyLevel = Enums.EmergencyLevel.Info, Description = "Notification 100" };

            // Act
            int actualNotificationBeforeInsertCount = testContext.Notifications.Count();
            notificationRepository.Insert(entityToInsert);
            testContext.SaveChanges();
            int actualNotificationAfterInsertCount = testContext.Notifications.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualNotificationBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualNotificationAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Notifications);
        }
        [Fact]
        public async Task InsertAsync()
        {
            // Arrange
            NotificationRepository notificationRepository = new NotificationRepository();
            notificationRepository.SetDbContext(testContext);

            int expectedEntitiesBeforeInsertCount = TestData.NotificationsCount;
            int expectedEntitiesAfterInsertCount = TestData.NotificationsCount + 1;

            Notification entityToInsert = new Notification { Id = 100, EmergencyLevel = Enums.EmergencyLevel.Info, Description = "Notification 100" };

            // Act
            int actualNotificationBeforeInsertCount = testContext.Notifications.Count();
            await notificationRepository.InsertAsync(entityToInsert);
            testContext.SaveChanges();
            int actualNotificationAfterInsertCount = testContext.Notifications.Count();

            // Assert
            Assert.Equal(expectedEntitiesBeforeInsertCount, actualNotificationBeforeInsertCount);
            Assert.Equal(expectedEntitiesAfterInsertCount, actualNotificationAfterInsertCount);
            Assert.Contains(entityToInsert, testContext.Notifications);
        }
        #endregion

    }
}

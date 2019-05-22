using Xunit;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Tests.TestDataInitializers;

namespace DataAccess.Tests.RepositoriesTest
{
    public class UserRepositoryTest : IDisposable
    {
        // FIELDS
        DataBaseContext testContext;
        UserTestDataInitializer testData;

        // CONSTRUCTORS
        public UserRepositoryTest()
        {
            testData = new UserTestDataInitializer();
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
        UserTestDataInitializer TestData => testData;

        // TESTS
        #region COUNT
        [Fact]
        public void CountTest()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int expectedUserCount = TestData.UsersCount;
            int expectedAdministratorCount = TestData.AdministratorCount;
            int expectedManagerCount = TestData.ManagersCount;
            int expectedResidentCount = TestData.ResidentCount;

            // Act
            int actualUserCount = userRepository.Count();
            int actualAdministratorCount = userRepository.Count(u => u.Role == Enums.Role.Administrator);
            int actualManagerCount = userRepository.Count(u => u.Role == Enums.Role.Manager);
            int actualResidentCount = userRepository.Count(u => u.Role == Enums.Role.Resident);

            // Assert
            Assert.Equal(expectedUserCount, actualUserCount);
            Assert.Equal(expectedAdministratorCount, actualAdministratorCount);
            Assert.Equal(expectedManagerCount, actualManagerCount);
            Assert.Equal(expectedResidentCount, actualResidentCount);
        }
        #endregion
        #region DELETE
        [Fact]
        public void DeleteByIdTest()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int idToDelete = testContext.Users.First().Id;
            int expectedCountBeforeDelete = TestData.UsersCount;
            int expectedCountAfterDelete = TestData.UsersCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            userRepository.Delete(idToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, x => x.Id == idToDelete);
        }
        [Fact]
        public void DeleteByObjectTest()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User userToDelete = testContext.Users.First();
            int expectedCountBeforeDelete = TestData.UsersCount;
            int expectedCountAfterDelete = TestData.UsersCount - 1;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            userRepository.Delete(userToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(userToDelete, testContext.Users);
        }

        [Fact]
        public void DeleteByPredicateTest()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            Expression<Func<User, bool>> predicateToDelete = u => u.Role == Enums.Role.Resident;
            int expectedCountBeforeDelete = TestData.UsersCount;
            int expectedCountAfterDelete = TestData.UsersCount - testData.ResidentCount;

            // Act
            int actualCountBeforeDelete = testContext.Users.Count();
            userRepository.Delete(predicateToDelete);
            testContext.SaveChanges();
            int actualCountAfterDelete = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedCountBeforeDelete, actualCountBeforeDelete);
            Assert.Equal(expectedCountAfterDelete, actualCountAfterDelete);
            Assert.DoesNotContain(testContext.Users, u => u.Role == Enums.Role.Resident);
        }
        #endregion
        #region GET
        [Fact]
        public void GetById()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User expectedUser = testContext.Users.First();
            int userId = expectedUser.Id;

            // Act
            User actualUser = userRepository.Get(userId);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User expectedUser = testContext.Users.First();
            int userId = expectedUser.Id;

            // Act
            User actualUser = await userRepository.GetAsync(userId);

            // Assert
            Assert.Equal(expectedUser, actualUser);
        }
        [Fact]
        public void GetByPredicate()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int expectedUsersCount = TestData.ResidentCount;
            Expression<Func<User, bool>> predicateToGet = u => u.Role == Enums.Role.Resident;
            IEnumerable<User> expectedUsers = testContext.Users.Where(predicateToGet);

            // Act
            IEnumerable<User> actualUsers = userRepository.Get(predicateToGet);
            int actualUsersCount = actualUsers.Count();

            // Assert
            Assert.Equal(expectedUsers, actualUsers);
            Assert.Equal(expectedUsersCount, actualUsersCount);
        }

        [Fact]
        public void GetByRole()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int expectedUsersCount = TestData.ResidentCount;
            Enums.Role roleToGet = Enums.Role.Resident;
            IEnumerable<User> expectedUsers = testContext.Users.Where(u => u.Role == roleToGet);

            // Act
            IEnumerable<User> actualUsers = userRepository.GetUserByRole(roleToGet);
            int actualUsersCount = actualUsers.Count();

            // Assert
            Assert.Equal(expectedUsers, actualUsers);
            Assert.Equal(expectedUsersCount, actualUsersCount);
        }
        #endregion
        #region INSERT
        [Fact]
        public void Insert()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int expectedUsersBeforeInsertCount = TestData.UsersCount;
            int expectedUsersAfterInsertCount = TestData.UsersCount + 1;

            User userToInsert = new User { Id = 100, FirstName = "Test", LastName = "Test", Email = "test.test@mail.com", Password = "1111", Role = Enums.Role.Resident };

            // Act
            int actualUsersBeforeInsertCount = testContext.Users.Count();
            userRepository.Insert(userToInsert);
            testContext.SaveChanges();
            int actualUsersAfterInsertCount = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedUsersBeforeInsertCount, actualUsersBeforeInsertCount);
            Assert.Equal(expectedUsersAfterInsertCount, actualUsersAfterInsertCount);
            Assert.Contains(userToInsert, testContext.Users);
        }
        [Fact]
        public async Task InsertAsync()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int expectedUsersBeforeInsertCount = TestData.UsersCount;
            int expectedUsersAfterInsertCount = TestData.UsersCount + 1;

            User userToInsert = new User { Id = 100, FirstName = "Test", LastName = "Test", Email = "test.test@mail.com", Password = "1111", Role = Enums.Role.Resident };

            // Act
            int actualUsersBeforeInsertCount = testContext.Users.Count();
            await userRepository.InsertAsync(userToInsert);
            testContext.SaveChanges();
            int actualUsersAfterInsertCount = testContext.Users.Count();

            // Assert
            Assert.Equal(expectedUsersBeforeInsertCount, actualUsersBeforeInsertCount);
            Assert.Equal(expectedUsersAfterInsertCount, actualUsersAfterInsertCount);
            Assert.Contains(userToInsert, testContext.Users);
        }
        #endregion
        #region USERS OPTION
        [Fact]
        public void DoesManagerHasAnyResident()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            int managerIdWithRenter = TestData.ManagerIdWithRenters;
            int managerIdWithoutRenter = TestData.ManagerIdWithoutRenters;

            User managerWithRenters = testContext.Users.Find(managerIdWithRenter);
            User managerWithoutRenters = testContext.Users.Find(managerIdWithoutRenter);

            bool expectedWithRenter = true;
            bool expectedWithouhtRenter = false;

            // Act
            bool actualWithRenter = userRepository.DoesManagerHasAnyResident(managerWithRenters);
            bool actualWithoutRenter = userRepository.DoesManagerHasAnyResident(managerWithoutRenters);

            // Assert
            Assert.Equal(expectedWithRenter, actualWithRenter);
            Assert.Equal(expectedWithouhtRenter, actualWithoutRenter);
        }

        [Fact]
        public void IsEmailFree()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            string freeEmail = "free.email@mail.com";
            bool expectedFreeEmailResult = true;

            string occupiedEmail = "john.doe@mail.com";
            bool expectedOccupiedEmailResult = false;

            // Act
            bool actualFreeEmailResult = userRepository.IsEmailFree(freeEmail);
            bool actualOccupiedEmailResult = userRepository.IsEmailFree(occupiedEmail);

            // Assert
            Assert.Equal(expectedOccupiedEmailResult, actualOccupiedEmailResult);
            Assert.Equal(expectedFreeEmailResult, actualFreeEmailResult);
        }
        [Fact]
        public void IsDataValid_ValidUser_AllTrue()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User validUser = testContext.Users.First();

            // Act
            Tuple<bool, bool, User> validationData = userRepository.IsDataValid(validUser.Email, validUser.Password);

            // Assert
            Assert.True(validationData.Item1);
            Assert.True(validationData.Item2);
            Assert.NotNull(validationData.Item3);
            Assert.Equal(validUser, validationData.Item3);
        }

        [Fact]
        public void IsDataValid_InvalidUser_WrongPassword()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User invalidUser = testContext.Users.First();
            string wrongPassword = invalidUser.Password + '1';

            // Act
            Tuple<bool, bool, User> validationData = userRepository.IsDataValid(invalidUser.Email, wrongPassword);

            // Assert
            Assert.True(validationData.Item1);
            Assert.False(validationData.Item2);
            Assert.Null(validationData.Item3);
        }

        [Fact]
        public void IsDataValid_InvalidUser_WrondEmail()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            User invalidUser = testContext.Users.First();
            string wrongEmail = invalidUser.Email + 'e';

            // Act
            Tuple<bool, bool, User> validationData = userRepository.IsDataValid(wrongEmail, invalidUser.Password);

            // Assert
            Assert.False(validationData.Item1);
            Assert.False(validationData.Item2);
            Assert.Null(validationData.Item3);
        }
        [Fact]
        public void IsLastIn()
        {
            // Arrange
            UserRepository userRepository = new UserRepository();
            userRepository.SetDbContext(testContext);

            Enums.Role lastRole = Enums.Role.Administrator;
            bool expectedIsLastRole = true;

            Enums.Role notLastRole = Enums.Role.Resident;
            bool expectedNotLastRole = false;

            // Act
            bool actualIsLastRole = userRepository.IsLastIn(lastRole);
            bool actualNotLastRole = userRepository.IsLastIn(notLastRole);

            // Assert
            Assert.Equal(expectedIsLastRole, actualIsLastRole);
            Assert.Equal(expectedNotLastRole, actualNotLastRole);
        }
        #endregion

    }
}

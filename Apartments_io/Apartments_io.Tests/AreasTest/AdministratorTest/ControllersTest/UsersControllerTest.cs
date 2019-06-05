using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Administrator.ViewModels.Users;
using Apartments_io.Areas.Administrator.Controllers;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Tests.AreasTest.AdministratorTest.ControllersTest
{
    public class UsersControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            int page = 1, itemPerPageSize = 10;
            IEnumerable<User> users = new List<User>
            {
                new User { FirstName = "John", Role = DataAccess.Enums.Role.Manager },
                new User { FirstName = "Alan", Role = DataAccess.Enums.Role.Resident }
            };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.Get(null, null, string.Empty, page, itemPerPageSize))
                .Returns(users);
            mockUserRepository
                .Setup(ur => ur.Count())
                .Returns(users.Count());

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = controller.Index();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Same(users, indexViewModel.Users);
        }

        [Fact]
        public void Managers_ViewResult()
        {
            // Arrange
            IEnumerable<User> residents = new List<User>
            {
                new User { FirstName = "Alan", Role = DataAccess.Enums.Role.Resident },
                new User { FirstName = "Eduard", Role = DataAccess.Enums.Role.Resident }
            };
            IEnumerable<User> managers = new List<User>
            {
                new User { FirstName = "John", Role = DataAccess.Enums.Role.Manager },
                new User { FirstName = "Ann", Role = DataAccess.Enums.Role.Manager }
            };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.Get(u => u.Role == DataAccess.Enums.Role.Resident, null, string.Empty, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(residents);
            mockUserRepository
                .Setup(ur => ur.GetUserByRole(DataAccess.Enums.Role.Manager))
                .Returns(managers);
            mockUserRepository
                .Setup(ur => ur.Count())
                .Returns(residents.Count());

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = controller.Managers();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ManagersViewModel managersViewModel = Assert.IsType<ManagersViewModel>(viewResult.Model);
            Assert.Same(residents, managersViewModel.Users);
            Assert.Same(managers, managersViewModel.Managers);
        }

        [Fact]
        public async void CreateEmailIsNotFree_BadRequest()
        {
            // Arrange
            User user = new User { Email = "first@gmail.com" };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.IsEmailFree(user.Email))
                .Returns(false);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Create(user);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email has already been taken", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void CreateEmailIsFree_OkResult()
        {
            // Arrange
            User user = new User { Email = "first@gmail.com" };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.IsEmailFree(user.Email))
                .Returns(true);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Create(user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Update_OkResult()
        {
            // Arrange
            User user = new User { Id = 1, Email = "first@gmail.com" };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();

            mockUserRepository
                .Setup(ur => ur.GetAsync(user.Id, It.IsAny<string>()))
                .Returns(Task.FromResult(user));
            
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateManagerNoUser_BadRequest()
        {
            // Arrange
            User resident = new User { Id = 1, Email = "first@gmail.com", Role = DataAccess.Enums.Role.Resident };
            User manager = new User { Id = 2, Email = "second@gmail.com", Role = DataAccess.Enums.Role.Manager };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(resident.Id, string.Empty))
                .Returns(Task.FromResult(null as User));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.UpdateManager(resident.Id, manager.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("There is no user with such id", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void UpdateManagerNoManager_BadRequest()
        {
            // Arrange
            User resident = new User { Id = 1, Email = "first@gmail.com", Role = DataAccess.Enums.Role.Resident };
            User manager = new User { Id = 2, Email = "second@gmail.com", Role = DataAccess.Enums.Role.Manager };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(resident.Id, string.Empty))
                .Returns(Task.FromResult(resident));
            mockUserRepository
                .Setup(ur => ur.GetAsync(manager.Id, string.Empty))
                .Returns(Task.FromResult(null as User));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.UpdateManager(resident.Id, manager.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("There is no manager with such id", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void UpdateManager_OkResult()
        {
            // Arrange
            User resident = new User { Id = 1, Email = "first@gmail.com", Role = DataAccess.Enums.Role.Resident };
            User manager = new User { Id = 2, Email = "second@gmail.com", Role = DataAccess.Enums.Role.Manager };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(resident.Id, string.Empty))
                .Returns(Task.FromResult(resident));
            mockUserRepository
                .Setup(ur => ur.GetAsync(manager.Id, string.Empty))
                .Returns(Task.FromResult(manager));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.UpdateManager(resident.Id, manager.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteManagerHasResidents_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, Email = "first@gmail.com" };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(user.Id, string.Empty))
                .Returns(Task.FromResult(user));
            mockUserRepository
                .Setup(ur => ur.DoesManagerHasAnyResident(user))
                .Returns(true);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Delete(user.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("You can not delete manager with renters", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void DeleteUserIsLastInHisRole_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, Email = "first@gmail.com", Role = DataAccess.Enums.Role.Manager };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(user.Id, string.Empty))
                .Returns(Task.FromResult(user));
            mockUserRepository
                .Setup(ur => ur.DoesManagerHasAnyResident(user))
                .Returns(false);
            mockUserRepository
                .Setup(ur => ur.IsLastIn(user.Role))
                .Returns(true);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Delete(user.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"You can not delete last {user.Role}", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void Delete_OkResult()
        {
            // Arrange
            User user = new User { Id = 1, Email = "first@gmail.com", Role = DataAccess.Enums.Role.Manager };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(user.Id, string.Empty))
                .Returns(Task.FromResult(user));
            mockUserRepository
                .Setup(ur => ur.DoesManagerHasAnyResident(user))
                .Returns(false);
            mockUserRepository
                .Setup(ur => ur.IsLastIn(user.Role))
                .Returns(false);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Delete(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}

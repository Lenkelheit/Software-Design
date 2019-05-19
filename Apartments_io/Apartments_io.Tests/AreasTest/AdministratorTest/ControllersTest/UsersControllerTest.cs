using Xunit;

using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Controllers;
using Apartments_io.ViewModels.Home;
using Apartments_io.Models;

using Moq;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Routing;
using Apartments_io.Areas.Administrator.ViewModels.Users;
using Apartments_io.Areas.Administrator.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                new User { FirstName = "John", Role = DataAccess.Enums.Role.Manager }, new User { FirstName = "Alan", Role = DataAccess.Enums.Role.Resident }
            };
            IEnumerable<User> managers = users.Where(u => u.Role == DataAccess.Enums.Role.Manager);

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.Get(null, null, string.Empty, page, itemPerPageSize))
                .Returns(users);
            mockUserRepository
                .Setup(ur => ur.GetUserByRole(DataAccess.Enums.Role.Manager))
                .Returns(managers);
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
            Assert.Equal(nameof(Areas.Administrator), viewResult.ViewData["Title"]);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Same(users, indexViewModel.Users);
            Assert.Same(managers, indexViewModel.Managers);
        }

        [Fact]
        public async void CreateEmailIsNotFree_BadRequest()
        {
            // Arrange
            User user = new User { Email = "first@gmail.com" };
            int managerId = 2;

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
            IActionResult result = await controller.Create(user, managerId);

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
            int managerId = 2;

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
            IActionResult result = await controller.Create(user, managerId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void Update_OkResult()
        {
            // Arrange
            User user = new User { Email = "first@gmail.com" };
            int managerId = 2;

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Update(user, managerId);

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

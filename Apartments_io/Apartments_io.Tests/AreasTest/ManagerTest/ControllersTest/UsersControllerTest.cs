using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using DataAccess.Wrappers;

using Apartments_io.Areas.Manager.Controllers;
using Apartments_io.Areas.Manager.ViewModels.Users;

using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ManagerTest.ControllersTest
{
    public class UsersControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<UserStatisticWrapper> userStatisticWrapper = new List<UserStatisticWrapper>
            {
                new UserStatisticWrapper { User = new User { Id = 1 } }
            };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetUserStatistics(It.IsAny<int>()))
                .Returns(userStatisticWrapper);

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            UsersController controller = new UsersController(mockIUnitOfWork.Object);

            Mock<ClaimsPrincipal> mockManager = new Mock<ClaimsPrincipal>();
            mockManager
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = mockManager.Object }
            };

            // Act
            IActionResult result = controller.Index();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Same(userStatisticWrapper, indexViewModel.UsersStatistic);
            Assert.Null(viewResult.ViewName);
        }
    }
}

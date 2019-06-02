using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Resident.Controllers;
using Apartments_io.Areas.Resident.ViewModels.Notifications;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ResidentTest.ControllersTest
{
    public class NotificationsControllerTest
    {
        [Fact]
        public void List_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Notification> notifications = new List<Notification>
            {
                new Notification { Id = 1 },
                new Notification { Id = 2 }
            };

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(nr => nr.Get(It.IsAny<Expression<Func<Notification, bool>>>(),
                                    It.IsAny<Func<IQueryable<Notification>, IOrderedQueryable<Notification>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(notifications);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            ControllerBase controller = new NotificationsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as NotificationsController)?.List();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ListViewModel listViewModel = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.Same(notifications, listViewModel.Notifications);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void ConfirmNotificationNotificationIsNull_BadRequest()
        {
            // Arrange
            Notification notification = new Notification { Id = 1 };

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(nr => nr.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Notification));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            NotificationsController controller = new NotificationsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.ConfirmNotification(notification.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No such notification", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void ConfirmNotification_OkResult()
        {
            // Arrange
            Notification notification = new Notification { Id = 1 };

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(nr => nr.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(notification));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            NotificationsController controller = new NotificationsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.ConfirmNotification(notification.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}

using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Resident.ViewComponents;

using System;
using System.Linq.Expressions;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ResidentTest.ViewComponentsTest
{
    public class NotificationsCountViewComponentTest
    {
        [Fact]
        public void Invoke_ContentViewComponentResult()
        {
            // Arrange
            int notificationsCount = 5, userId = 1;

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(nr => nr.Count(It.IsAny<Expression<Func<Notification, bool>>>()))
                .Returns(notificationsCount);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            NotificationsCountViewComponent viewComponent = new NotificationsCountViewComponent(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            viewComponent.ViewComponentContext = new ViewComponentContext
            {
                ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = userMock.Object
                    }
                }
            };

            // Act
            IViewComponentResult result = viewComponent.Invoke();

            // Assert
            Assert.NotNull(result);
            ContentViewComponentResult contentViewComponentResult = Assert.IsType<ContentViewComponentResult>(result);
            Assert.NotNull(contentViewComponentResult.Content);
            Assert.Equal(notificationsCount.ToString(), contentViewComponentResult.Content);
        }
    }
}

using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Manager.Controllers;
using Apartments_io.Areas.Manager.ViewModels.Requests;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Apartments_io.Tests.AreasTest.ManagerTest.ControllersTest
{
    public class RequestControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Request> requests = new List<Request>
            {
                new Request { Id = 5 }
            };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(rr => rr.GetShortInfo(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(requests);

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

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
            Assert.Equal(requests, indexViewModel.Requests);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void AcceptRequestRequestIsNull_BadRequest()
        {
            // Arrange
            Request request = new Request { Id = 5 };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(rr => rr.Get(It.IsAny<Expression<Func<Request, bool>>>(),
                                    It.IsAny<Func<IQueryable<Request>, IOrderedQueryable<Request>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(null as IEnumerable<Request>);
            
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.AcceptRequest(request.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No request found", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void AcceptRequest_OkResult()
        {
            // Arrange
            int requestId = 1;
            IEnumerable<Request> requests = new List<Request>
            {
                new Request { Id = 5, Apartment = new Apartment { Renter = new User { Id = 1 } } }
            };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>(MockBehavior.Loose);
            mockRequestRepository
                .Setup(rr => rr.Get(It.IsAny<Expression<Func<Request, bool>>>(),
                                    null,
                                    It.IsAny<string>(),
                                    null, null))
                .Returns(requests);
            mockRequestRepository
                .Setup(rr => rr.Delete(It.IsAny<Expression<Func<Request, bool>>>()));

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(n => n.InsertAsync(It.IsAny<Notification>()))
                .Returns(System.Threading.Tasks.Task.FromResult(null as Notification));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.AcceptRequest(requestId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DismissRequest_OkResult()
        {
            // Arrange
            Request request = new Request { Id = 5 };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(rr => rr.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(request));
            mockRequestRepository
                .Setup(rr => rr.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(request);

            Mock<GenericRepository<Notification>> mockNotificationRepository = new Mock<GenericRepository<Notification>>();
            mockNotificationRepository
                .Setup(n => n.InsertAsync(It.IsAny<Notification>()))
                .Returns(System.Threading.Tasks.Task.FromResult(null as Notification));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Notification, GenericRepository<Notification>>())
                .Returns(mockNotificationRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DismissRequest(request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}

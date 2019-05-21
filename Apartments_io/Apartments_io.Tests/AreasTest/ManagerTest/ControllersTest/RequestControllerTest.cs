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
using System.Threading.Tasks;
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
            //Arange
            IEnumerable<Request> requests = new List<Request>
            {
                new Request {Id = 5}
            };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(i => i.Get(null, null, null, null, null))
                .Returns(requests);
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            ControllerBase controller = new RequestsController(mockIUnitOfWork.Object);

            Mock<ClaimsPrincipal> mockManager = new Mock<ClaimsPrincipal>();
            mockManager
                .Setup(i => i.FindFirst(It.IsAny<string>()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = mockManager.Object }
            };

            // //Act
            IActionResult result = (controller as RequestsController)?.Index();

            //Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Equal(requests, indexViewModel.Requests);
        }
        [Fact]
        public async void AcceptRequestRequestIsNull_BadRequest()
        {
            // Arange
            Request request = new Request { Id = 5 };
            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(i => i.Get(request.Id, It.IsAny<string>()))
                .Returns(request);           
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Request, RequestRepository>())
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
        public async void AcceptRequestDeleteRequest_OkResult()
        {
            // Arange
            Request request = new Request { Id = 5 };
            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                 .Setup(i => i.Delete(request.Id));
                 
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.AcceptRequest(request.Id);

            //Asesert
            Assert.NotNull(result);
            Assert.IsNotType<OkResult>(result);
        }
        [Fact]
        public async void DismissRequest_OkResult()
        {
            // Arange
            Request request = new Request { Id = 5 };
            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(i => i.Delete(request.Id));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockIUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DismissRequest(request.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}

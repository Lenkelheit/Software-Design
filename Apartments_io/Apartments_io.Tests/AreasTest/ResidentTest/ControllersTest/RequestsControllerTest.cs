using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Resident.Controllers;
using Apartments_io.Areas.Resident.ViewModels.Requests;

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
    public class RequestsControllerTest
    {
        [Fact]
        public void List_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Request> requests = new List<Request>
            {
                new Request { Id = 1 },
                new Request { Id = 2 }
            };

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(rr => rr.Get(It.IsAny<Expression<Func<Request, bool>>>(),
                                    It.IsAny<Func<IQueryable<Request>, IOrderedQueryable<Request>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(requests);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            ControllerBase controller = new RequestsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as RequestsController)?.List();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ListViewModel listViewModel = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.Equal(userId, listViewModel.UserId);
            Assert.Same(requests, listViewModel.Requests);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void CreateRequestUserIsNull_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as User));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.CreateRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User does not exist", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void CreateRequestApartmentIsNull_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.CreateRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Apartment does not exist", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void CreateRequest_OkResult()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.CreateRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteRequestUserIsNull_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as User));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DeleteRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User does not exist", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void DeleteRequestApartmentIsNull_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DeleteRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Apartment does not exist", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void DeleteRequestRequestIsNull_BadRequest()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(ur => ur.GetByValues(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(null as Request);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DeleteRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Request does not exist", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void DeleteRequest_OkResult()
        {
            // Arrange
            User user = new User { Id = 1, FirstName = "Den" };
            Apartment apartment = new Apartment { Id = 1 };
            Request request = new Request { Id = 1 };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ur => ur.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<RequestRepository> mockRequestRepository = new Mock<RequestRepository>();
            mockRequestRepository
                .Setup(ur => ur.GetByValues(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(request);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            mockUnitOfWork
                .Setup(u => u.GetRepository<Request, RequestRepository>())
                .Returns(mockRequestRepository.Object);

            RequestsController controller = new RequestsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.DeleteRequest(user.Id, apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }
    }
}

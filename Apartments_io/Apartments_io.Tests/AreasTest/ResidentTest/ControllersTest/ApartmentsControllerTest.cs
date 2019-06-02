using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Resident.Controllers;
using Apartments_io.Areas.Resident.ViewModels.Apartments;

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
    public class ApartmentsControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            IEnumerable<Apartment> apartments = new List<Apartment>
            {
                new Apartment { Id = 1 },
                new Apartment { Id = 2 }
            };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.GetRandom(It.IsAny<int>()))
                .Returns(apartments);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = controller.Index();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IEnumerable<Apartment> apartmentsModel = Assert.IsAssignableFrom<IEnumerable<Apartment>>(viewResult.Model);
            Assert.Same(apartments, apartmentsModel);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void List_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Apartment> apartments = new List<Apartment>
            {
                new Apartment { Id = 1 },
                new Apartment { Id = 2 }
            };

            ISet<int> apartmentsIds = apartments.Select(a => a.Id).ToHashSet();

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.Get(It.IsAny<Expression<Func<Apartment, bool>>>(),
                                    It.IsAny<Func<IQueryable<Apartment>, IOrderedQueryable<Apartment>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(apartments);
            mockApartmentRepository
                .Setup(ar => ar.HasRequests(It.IsAny<int>(), It.IsAny<IEnumerable<int>>()))
                .Returns(apartmentsIds);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            ControllerBase controller = new ApartmentsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as ApartmentsController)?.List(null, null);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ListViewModel listViewModel = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.Equal(userId, listViewModel.UserId);
            Assert.Same(apartments, listViewModel.Apartments);
            Assert.Same(apartmentsIds, listViewModel.IsUsersRequest);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void MyRent_ViewResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Apartment> apartments = new List<Apartment>
            {
                new Apartment { Id = 1 },
                new Apartment { Id = 2 }
            };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.Get(It.IsAny<Expression<Func<Apartment, bool>>>(),
                                    It.IsAny<Func<IQueryable<Apartment>, IOrderedQueryable<Apartment>>>(),
                                    It.IsAny<string>(), It.IsAny<int>(),
                                    It.IsAny<int>()))
                .Returns(apartments);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            ControllerBase controller = new ApartmentsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as ApartmentsController)?.MyRent();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            ListViewModel listViewModel = Assert.IsType<ListViewModel>(viewResult.Model);
            Assert.Same(apartments, listViewModel.Apartments);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void SingleApartmentIdIsNull_NotFoundResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Single(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void SingleApartmentIsNull_NotFoundResult()
        {
            // Arrange
            int apartmentId = 1;
            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Single(apartmentId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Single_ViewResult()
        {
            // Arrange
            int userId = 1, apartmentId = 1;
            bool isRenter = true, hasRequest = false;
            Apartment apartment = new Apartment { Id = 1 };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));
            mockApartmentRepository
                .Setup(ar => ar.IsRenter(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(isRenter);
            mockApartmentRepository
                .Setup(ar => ar.HasRequest(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(hasRequest);

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);

            ControllerBase controller = new ApartmentsController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock
                .Setup(p => p.FindFirst(It.IsAny<string>()))
                .Returns(new Claim(nameof(User.Id), userId.ToString()));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = await (controller as ApartmentsController)?.Single(apartmentId);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            SingleViewModel singleViewModel = Assert.IsType<SingleViewModel>(viewResult.Model);
            Assert.Equal(userId, singleViewModel.UserId);
            Assert.Same(apartment, singleViewModel.Apartment);
            Assert.Equal(isRenter, singleViewModel.IsRenter);
            Assert.Equal(hasRequest, singleViewModel.HasUserRequest);
            Assert.Null(viewResult.ViewName);
        }
    }
}

using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Manager.Controllers;
using Apartments_io.Areas.Manager.ViewModels.Apartments;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Tests.AreasTest.ManagerTest.ControllersTest
{
    public class ApartamentsControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arrange
            int? daysToFree = 5;
            IEnumerable<Apartment> apartments = new List<Apartment>()
            {
                new Apartment { Name = "NewApartament", Renter = new User {FirstName = "Jeson" } }
            };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.Get(It.IsAny<Expression<Func<Apartment, bool>>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(apartments);
          
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = controller.Index(daysToFree, isFree: null);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Equal(apartments, indexViewModel.Apartments);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void DetailsIdIsNull_NotFoundResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Details(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DetailsApartamentIsNull_NotFoundResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Details(apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Details_ViewResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7 };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Details(apartment.Id);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Apartment ap = Assert.IsType<Apartment>(viewResult.Model);
            Assert.Equal(apartment, ap);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Create_ViewResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IFileService> mockIFileService = new Mock<IFileService>();
            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = controller.Create();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void CreateModelIsNotValid_ViewResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 1 };

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IFileService> mockIFileService = new Mock<IFileService>();
            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);
            controller.ModelState.AddModelError("RentEndDate", "Bad date!");

            // Act
            IActionResult result = await controller.Create(apartment, null);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Apartment apartmentModel = Assert.IsType<Apartment>(viewResult.Model);
            Assert.Same(apartment, apartmentModel);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Create_RedirectToAction()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 1 };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Create(apartment, null);

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(ApartmentsController.Index), redirect.ActionName);
            Assert.Null(redirect.ControllerName);
        }

        [Fact]
        public async void EditIdIsNull_NotFoundResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void EditApartmentIsNull_NotFoundResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Edit_ViewResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(apartment.Id);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Apartment apartmentModel = Assert.IsType<Apartment>(viewResult.Model);
            Assert.Same(apartment, apartmentModel);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void EditIdsAreDifferent_NotFoundResult()
        {
            // Arrange
            int id = 1;
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(id, apartment, null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void EditModelIsNotValid_ViewResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 1 };

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IFileService> mockIFileService = new Mock<IFileService>();
            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);
            controller.ModelState.AddModelError("RentEndDate", "Bad date!");

            // Act
            IActionResult result = await controller.Edit(apartment.Id, apartment, null);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Apartment apartmentModel = Assert.IsType<Apartment>(viewResult.Model);
            Assert.Same(apartment, apartmentModel);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void Edit_RedirectToAction()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 1 };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(apartment.Id, apartment, null);

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(ApartmentsController.Index), redirect.ActionName);
            Assert.Null(redirect.ControllerName);
        }

        [Fact]
        public async void DeleteIdIsNull_NotFoundResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();

            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Delete(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteApartmentIsNull_NotFoundResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(null as Apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Delete(apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Delete_ViewResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Delete(apartment.Id);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Apartment apartmentModel = Assert.IsType<Apartment>(viewResult.Model);
            Assert.Same(apartment, apartmentModel);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void DeleteConfirmed_RedirectToActionResult()
        {
            // Arrange
            Apartment apartment = new Apartment
            {
                Id = 1,
                Description = "nice",
                Price = 50
            };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ap => ap.GetAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = await controller.DeleteConfirmed(apartment.Id);

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(ApartmentsController.Index), redirect.ActionName);
            Assert.Null(redirect.ControllerName);
        }

        [Fact]
        public void GetApartmentsList_OkObjectResult()
        {
            // Arrange
            int userId = 1;
            IEnumerable<Apartment> apartments = new List<Apartment>
            {
                new Apartment { Id = 5, Name = "Avalon" }
            };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ap => ap.Get(It.IsAny<Expression<Func<Apartment, bool>>>(),
                                    null,
                                    null, null,
                                    null))
                .Returns(apartments);

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = controller.GetApartmentsList(userId);

            // Assert
            Assert.NotNull(result);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult.Value);
            Assert.IsAssignableFrom<IEnumerable<Apartment>>(okObjectResult.Value);
        }

        [Fact]
        public void GetApartmentImage_OkObjectResult()
        {
            // Arrange
            Apartment apartment = new Apartment { Id = 5, Name = "Avalon", Price = 100, MainPhoto = "house.jpg" };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(ar => ar.GetImageById(It.IsAny<int>()))
                .Returns(apartment.MainPhoto);

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(u => u.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = controller.GetApartmentImage(apartment.Id);

            // Assert
            Assert.NotNull(result);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult.Value);
            string mainPhotoModel = Assert.IsType<string>(okObjectResult.Value);
            Assert.Same(apartment.MainPhoto, mainPhotoModel);
        }
    }
}

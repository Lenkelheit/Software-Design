using Xunit;

using Moq;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Areas.Manager.Controllers;
using Apartments_io.Areas.Manager.ViewModels.Apartments;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Tests.AreasTest.ManagerTest.ControllersTest
{
    public class ApartamentsControllerTest
    {
        [Fact]
        public void Index_ViewResult()
        {
            // Arange
            int? daysToFree = 5;
            IEnumerable<Apartment> apartments = new List<Apartment>()
            {
                new Apartment { Name = "NewApartament", Renter = new User {FirstName = "Jeson" } }
            };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.Get(It.IsAny<System.Linq.Expressions.Expression<System.Func<Apartment, bool>>>(), null, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(apartments);
          
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = controller.Index(daysToFree);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);// indexViewModel
            IndexViewModel indexViewModel = Assert.IsType<IndexViewModel>(viewResult.Model);
            Assert.Equal(apartments, indexViewModel.Apartments);
            Assert.Null(viewResult.ViewName);
        }
        [Fact]
        public async void DetailsIdIsNull_NotFoundResult()
        {
            // Arange
            int? id = 7;
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };
           
            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(id.Value, It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Details(id.Value) as NotFoundResult;

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void DetailsApartamentIsNull_NotFoundResult()
        {
            // Arange            
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(apartment.Id, It.IsAny<string>()))
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
        public async void DetailsCreate_ViewResult()
        {
            // Arange            
            Apartment apartment = new Apartment { Id = 7 };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(apartment.Id, It.IsAny<string>()))
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
        }
        [Fact]
        public void Create_ViewResult()
        {
            // Arange
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            Mock<IFileService> mockIFileService = new Mock<IFileService>();
            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            ViewResult result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
        //[Fact]
        //public async void CreateIndex_RedirectToAction()
        //{
        //    // Arange
        //    Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
        //    Mock<IFileService> mockIFileService = new Mock<IFileService>();
        //    ControllerBase controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

        //    // Act
        //    IActionResult result = await (controller as ApartmentsController)?.Index(It.IsAny<int>());

        //    // Assert
        //    Assert.NotNull(result);
        //    RedirectToActionResult redirect = Assert.IsType<RedirectToActionResult>(result);
        //    Assert.Equal(nameof(Areas.Manager.Controllers.ApartmentsController.Index), redirect.ActionName);
        //}
        [Fact]
        public async void EditsIdIsNull_NotFoundResult()
        {
            // Arange
            int? id = 7;
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(id.Value, It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Edit(id.Value) as NotFoundResult;

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void EditApartamentIsNull_NotFoundResult()
        {
            // Arange            
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(apartment.Id, It.IsAny<string>()))
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
        public async void DeletesIdIsNull_NotFoundResult()
        {
            // Arange
            int? id = 7;
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(id.Value, It.IsAny<string>()))
                .Returns(Task.FromResult(apartment));

            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(uw => uw.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartamentRepository.Object);
            Mock<IFileService> mockIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIFileService.Object);

            // Act
            IActionResult result = await controller.Delete(id.Value) as NotFoundResult;

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public async void DeleteApartamentIsNull_NotFoundResult()
        {
            // Arange            
            Apartment apartment = new Apartment { Id = 7, Name = "NewApartament" };

            Mock<ApartmentRepository> mockApartamentRepository = new Mock<ApartmentRepository>();
            mockApartamentRepository
                .Setup(ar => ar.GetAsync(apartment.Id, It.IsAny<string>()))
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
        public async void DeleteConfirmed_RedirectToActionResult()
        {
            // Arange
            Apartment apartment = new Apartment
            {
                Id = 1,
                Description = "nice",
                Price = 50
            };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(i => i.GetAsync(apartment.Id, string.Empty))
                .Returns(Task.FromResult(apartment));
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(i => i.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = await controller.DeleteConfirmed(apartment.Id);

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        }
        [Fact]
        public void GetApartmentsList_OkObjectResult()
        {
            // Arange
            User rentner = new User { Id = 5, FirstName = "Leo" };
            Apartment apartment = new Apartment { Id = 5, Name = "Avalon", Renter = rentner, Price = 100 };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(i => i.Get(rentner.Id, string.Empty))
                .Returns(apartment);
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = controller.GetApartmentsList(rentner.Id);

            // Assert
            Assert.NotNull(result);
            OkObjectResult ok = Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public void GetApartmentImage_OkObjectResult()
        {
            // Arange
            Apartment apartment = new Apartment { Id = 5, Name = "Avalon", Price = 100 };

            Mock<ApartmentRepository> mockApartmentRepository = new Mock<ApartmentRepository>();
            mockApartmentRepository
                .Setup(i => i.Get(apartment.Id, string.Empty))
                .Returns(apartment);
            Mock<IUnitOfWork> mockIUnitOfWork = new Mock<IUnitOfWork>();
            mockIUnitOfWork
                .Setup(i => i.GetRepository<Apartment, ApartmentRepository>())
                .Returns(mockApartmentRepository.Object);
            Mock<IFileService> mockIIFileService = new Mock<IFileService>();

            ApartmentsController controller = new ApartmentsController(mockIUnitOfWork.Object, mockIIFileService.Object);

            // Act
            IActionResult result = controller.GetApartmentImage(apartment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}

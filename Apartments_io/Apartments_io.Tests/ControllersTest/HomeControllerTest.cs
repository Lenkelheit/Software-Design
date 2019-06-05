using Xunit;

using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Apartments_io.Models;
using Apartments_io.Controllers;
using Apartments_io.ViewModels.Home;

using Moq;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authentication;

namespace Apartments_io.Tests.ControllersTest
{
    public class HomeControllerTest
    {
        [Fact]
        public void IndexIsNotUserAuthenticated_ViewResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(false);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void IndexIsUserAdministrator_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Administrator))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Administrator.Controllers.UsersController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Administrator.Controllers.UsersController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Administrator), redirectToActionResult.RouteValues[$"{nameof(Areas).Remove("s")}"]);
        }

        [Fact]
        public void IndexIsUserManager_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Manager))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Manager.Controllers.ApartmentsController.CheckApartmentState), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Manager.Controllers.ApartmentsController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Manager), redirectToActionResult.RouteValues[$"{nameof(Areas).Remove("s")}"]);
        }

        [Fact]
        public void IndexIsUserResident_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Resident))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Resident.Controllers.ApartmentsController.CheckApartmentState), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Resident.Controllers.ApartmentsController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Resident), redirectToActionResult.RouteValues[$"{nameof(Areas).Remove("s")}"]);
        }

        [Fact]
        public void IndexIsUserDeactivated_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            HomeController controller = new HomeController(mockUnitOfWork.Object);

            Mock<ClaimsPrincipal> userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Deactivated))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            IActionResult result = controller.Index();

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Resident.Controllers.SiteController.Deactivated), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Resident.Controllers.SiteController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Resident), redirectToActionResult.RouteValues["Area"]);
        }

        [Fact]
        public async void LoginModelIsNotValid_ViewResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            HomeController controller = new HomeController(mockUnitOfWork.Object);

            controller.ModelState.AddModelError("Email", "Bad email!");
            LoginViewModel model = new LoginViewModel();

            // Act
            IActionResult result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<LoginViewModel>(viewResult.Model);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async void LoginEmailAndPasswordAreNull_BadRequest()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = null, Password = null };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.IsDataValid(model.Email, model.Password))
                .Returns(new Tuple<bool, bool, User>(false, false, null));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            HomeController controller = new HomeController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Wrong email", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void LoginOnlyPasswordAreNull_BadRequest()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = "first@gmail.com", Password = null };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.IsDataValid(model.Email, model.Password))
                .Returns(new Tuple<bool, bool, User>(true, false, null));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            HomeController controller = new HomeController(mockUnitOfWork.Object);

            // Act
            IActionResult result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            BadRequestObjectResult badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Wrong password", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void LoginSuccessful_RedirectToActionResult()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = "first@gmail.com", Password = "1111" };

            Mock<UserRepository> mockUserRepository = new Mock<UserRepository>();
            mockUserRepository
                .Setup(ur => ur.IsDataValid(model.Email, model.Password))
                .Returns(new Tuple<bool, bool, User>(true, true, new User { Email = model.Email, Password = model.Password }));

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>())
                .Returns(mockUserRepository.Object);

            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<IAuthenticationService> authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(ser => ser.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(ser => ser.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            Mock<IUrlHelperFactory> urlHelperFactory = new Mock<IUrlHelperFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
            };

            // Act
            IActionResult result = await (controller as HomeController)?.Login(model);

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(HomeController).Remove("Controller"), redirectToActionResult.ControllerName);
        }

        [Fact]
        public async void Logout_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<IAuthenticationService> authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(ser => ser.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(ser => ser.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            Mock<IUrlHelperFactory> urlHelperFactory = new Mock<IUrlHelperFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
            };

            // Act
            IActionResult result = await (controller as HomeController)?.Logout();

            // Assert
            Assert.NotNull(result);
            RedirectToActionResult redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(HomeController).Remove("Controller"), redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Error_ViewResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            Mock<HttpContext> httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(httpCon => httpCon.TraceIdentifier).Returns(string.Empty);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            IActionResult result = (controller as HomeController)?.Error();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}

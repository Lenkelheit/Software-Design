using Xunit;

using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Repositories;
using DataAccess.Interfaces;

using Apartments_io.Controllers;
using Apartments_io.ViewModels.Home;
using Apartments_io.Models;

using Moq;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Routing;

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

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(false);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            var result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void IndexIsUserAdministrator_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Administrator))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            var result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
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

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Manager))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            var result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Manager.Controllers.ApartmentsController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Manager.Controllers.ApartmentsController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Manager), redirectToActionResult.RouteValues[$"{nameof(Areas).Remove("s")}"]);
        }

        [Fact]
        public void IndexIsUserResident_RedirectToActionResult()
        {
            // Arrange
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            var userMock = new Mock<ClaimsPrincipal>();
            userMock.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            userMock.Setup(p => p.IsInRole(nameof(DataAccess.Enums.Role.Resident))).Returns(true);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = userMock.Object }
            };

            // Act
            var result = (controller as HomeController)?.Index();

            // Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Areas.Resident.Controllers.ApartmentsController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(Areas.Resident.Controllers.ApartmentsController).Remove("Controller"), redirectToActionResult.ControllerName);
            Assert.Equal(nameof(Areas.Resident), redirectToActionResult.RouteValues[$"{nameof(Areas).Remove("s")}"]);
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
            var result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<LoginViewModel>(viewResult.Model);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async void LoginEmailAndPasswordAreNull_BadRequest()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = null, Password = null };

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.GetRepository<User, UserRepository>().IsDataValid(model.Email, model.Password))
                .Returns(new System.Tuple<bool, bool, User>(false, false, null));
            HomeController controller = new HomeController(mockUnitOfWork.Object);

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Wrong email", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void LoginOnlyPasswordAreNull_BadRequest()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = "first@gmail.com", Password = null };

            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.GetRepository<User, UserRepository>().IsDataValid(model.Email, model.Password))
                .Returns(new Tuple<bool, bool, User>(true, false, null));
            HomeController controller = new HomeController(mockUnitOfWork.Object);

            // Act
            var result = await controller.Login(model);

            // Assert
            Assert.NotNull(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Wrong password", badRequestObjectResult.Value.ToString());
        }

        [Fact]
        public async void LoginSuccessful_RedirectToActionResult()
        {
            // Arrange
            LoginViewModel model = new LoginViewModel { Email = "first@gmail.com", Password = "1111" };

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(u => u.GetRepository<User, UserRepository>().IsDataValid(model.Email, model.Password))
                .Returns(new Tuple<bool, bool, User>(true, true, new User { Email = model.Email, Password = model.Password }));

            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(ser => ser.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<Microsoft.AspNetCore.Authentication.AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(ser => ser.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
            };

            // Act
            var result = await (controller as HomeController)?.Login(model);

            // Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(HomeController).Remove("Controller"), redirectToActionResult.ControllerName);
        }

        [Fact]
        public async void Logout_RedirectToActionResult()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(ser => ser.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<Microsoft.AspNetCore.Authentication.AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(ser => ser.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { RequestServices = serviceProviderMock.Object }
            };

            // Act
            var result = await (controller as HomeController)?.Logout();

            // Assert
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);
            Assert.Equal(nameof(HomeController).Remove("Controller"), redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Error_ViewResult()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            ControllerBase controller = new HomeController(mockUnitOfWork.Object);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(httpCon => httpCon.TraceIdentifier).Returns(string.Empty);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            var result = (controller as HomeController)?.Error();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }
    }
}

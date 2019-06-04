using Xunit;

using Apartments_io.Areas.Resident.Controllers;
using Apartments_io.Areas.Resident.ViewModels.Site;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Tests.AreasTest.ResidentTest.ControllersTest
{
    public class SiteControllerTest
    {
        [Fact]
        public void About_ViewResult()
        {
            // Arrange
            SiteController controller = new SiteController();

            // Act
            IActionResult result = controller.About();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            TemplateViewModel templateViewModel = Assert.IsType<TemplateViewModel>(viewResult.Model);
            Assert.Equal("About", templateViewModel.Header);
            Assert.Equal("Lorem ipsum", templateViewModel.Text);
            Assert.Equal("Template", viewResult.ViewName);
        }

        [Fact]
        public void Privacy_ViewResult()
        {
            // Arrange
            SiteController controller = new SiteController();

            // Act
            IActionResult result = controller.Privacy();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            TemplateViewModel templateViewModel = Assert.IsType<TemplateViewModel>(viewResult.Model);
            Assert.Equal("Privacy", templateViewModel.Header);
            Assert.Equal("Lorem ipsum", templateViewModel.Text);
            Assert.Equal("Template", viewResult.ViewName);
        }

        [Fact]
        public void Support_ViewResult()
        {
            // Arrange
            SiteController controller = new SiteController();

            // Act
            IActionResult result = controller.Support();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            TemplateViewModel templateViewModel = Assert.IsType<TemplateViewModel>(viewResult.Model);
            Assert.Equal("Support", templateViewModel.Header);
            Assert.Equal("Lorem ipsum", templateViewModel.Text);
            Assert.Equal("Template", viewResult.ViewName);
        }

        [Fact]
        public void Contact_ViewResult()
        {
            // Arrange
            SiteController controller = new SiteController();

            // Act
            IActionResult result = controller.Contact();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            TemplateViewModel templateViewModel = Assert.IsType<TemplateViewModel>(viewResult.Model);
            Assert.Equal("Contact", templateViewModel.Header);
            Assert.Equal("Lorem ipsum", templateViewModel.Text);
            Assert.Equal("Template", viewResult.ViewName);
        }

        [Fact]
        public void Deactivated_ViewResult()
        {
            // Arrange
            SiteController controller = new SiteController();

            // Act
            IActionResult result = controller.Deactivated();

            // Assert
            Assert.NotNull(result);
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            TemplateViewModel templateViewModel = Assert.IsType<TemplateViewModel>(viewResult.Model);
            Assert.Equal("Deactivated", templateViewModel.Header);
            Assert.Equal("Your account has been disabled. Please, contact us for further information.", templateViewModel.Text);
            Assert.Equal("Template", viewResult.ViewName);
        }
    }
}

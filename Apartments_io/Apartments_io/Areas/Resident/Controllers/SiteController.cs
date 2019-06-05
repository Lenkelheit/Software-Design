using Microsoft.AspNetCore.Mvc;

using Apartments_io.Attributes;
using Apartments_io.Areas.Resident.ViewModels.Site;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
    [Roles(nameof(DataAccess.Enums.Role.Resident), nameof(DataAccess.Enums.Role.Deactivated))]
    public class SiteController : Controller
    {
        // ACTIONS
        public IActionResult About()
        {
            return View("Template", new TemplateViewModel()
            {
                Header = "About",
                Text = "Lorem ipsum"
            });
        }

        public IActionResult Privacy()
        {
            return View("Template", new TemplateViewModel()
            {
                Header = "Privacy",
                Text = "Lorem ipsum"
            });
        }
        public IActionResult Support()
        {
            return View("Template", new TemplateViewModel()
            {
                Header = "Support",
                Text = "Lorem ipsum"
            });
        }
        public IActionResult Contact()
        {
            return View("Template", new TemplateViewModel()
            {
                Header = "Contact",
                Text = "Lorem ipsum"
            });
        }

        [Roles(nameof(DataAccess.Enums.Role.Deactivated))]
        public IActionResult Deactivated()
        {
            return View("Template", new TemplateViewModel()
            {
                Header = "Deactivated",
                Text = "Your account has been disabled. Please, contact us for further information."
            });
        }
    }
}
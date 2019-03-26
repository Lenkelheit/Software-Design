using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

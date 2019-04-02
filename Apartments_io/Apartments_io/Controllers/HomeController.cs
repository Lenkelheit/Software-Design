using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Controllers
{
    public class HomeController : Controller
    {
        DataAccess.Interfaces.IUnitOfWork unitOfWork;
        public HomeController(DataAccess.Interfaces.IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewData["Users"] = unitOfWork.GetRepository<User, GenericRepository<User>>().Get();

            return View();
        }
    }
}

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class AdminPagesController : Controller
    {
        // FIELDS
        readonly IUnitOfWork unitOfWork;
        readonly IRepository<User> userRepository;

        // CONSTRUCTORS
        public AdminPagesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.GetRepository<User, GenericRepository<User>>();
        }

        // ACTIONS
        public IActionResult Index()
        {
            ViewData["Title"] = "Administrator";
            ViewData["Users"] = userRepository.Get();

            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            ViewData["Title"] = "Administrator";

            userRepository.Insert(user);
            try
            {
                unitOfWork.Save();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                ViewData["Exception"] = "All fields are required. Maybe another exception has been occured";
            }

            ViewData["Users"] = userRepository.Get();

            return View(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            ViewData["Title"] = "Administrator";

            userRepository.Delete(id);
            unitOfWork.Save();

            ViewData["Users"] = userRepository.Get();

            return View(nameof(Index));
        }
    }
}
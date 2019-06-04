using Core.Extensions;

using Apartments_io.Models;
using Apartments_io.ViewModels.Home;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Apartments_io.Controllers
{
    public class HomeController : Controller
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;

        // CONSTRUCTORS
        public HomeController(IUnitOfWork unitOfWork)
        {
            this.ViewData["Title"] = "Apartments.io";
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole(nameof(DataAccess.Enums.Role.Administrator)))
                {
                    return this.RedirectToAction(
                        areaName: nameof(Areas.Administrator),
                        actionName: nameof(Areas.Administrator.Controllers.UsersController.Index),
                        controllerName: nameof(Areas.Administrator.Controllers.UsersController).Remove("Controller"));
                }
                else if (User.IsInRole(nameof(DataAccess.Enums.Role.Manager)))
                {
                    return this.RedirectToAction(
                        areaName: nameof(Areas.Manager),
                        actionName: nameof(Areas.Manager.Controllers.ApartmentsController.CheckApartmentState),
                        controllerName: nameof(Areas.Manager.Controllers.ApartmentsController).Remove("Controller"));
                }
                else if (User.IsInRole(nameof(DataAccess.Enums.Role.Resident)))
                {
                    return this.RedirectToAction(
                        areaName: nameof(Areas.Resident),
                        actionName: nameof(Areas.Resident.Controllers.ApartmentsController.CheckApartmentState),
                        controllerName: nameof(Areas.Resident.Controllers.ApartmentsController).Remove("Controller"));
                }
                else if (User.IsInRole(nameof(DataAccess.Enums.Role.Deactivated)))
                {
                    return this.RedirectToAction(
                        areaName: nameof(Areas.Resident),
                        actionName: nameof(Areas.Resident.Controllers.SiteController.Deactivated),
                        controllerName: nameof(Areas.Resident.Controllers.SiteController).Remove("Controller"));
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check data
                System.Tuple<bool, bool, User> userData = userRepository.IsDataValid(model.Email, model.Password);
                if (userData.Item1 == false) return BadRequest("Wrong email");
                if (userData.Item2 == false) return BadRequest("Wrong password");
                if (userData.Item3 ==  null) return BadRequest("Could not get user");

                // Authenticate
                await Authenticate(userData.Item3); 

                return RedirectToAction(actionName: nameof(Index), controllerName: nameof(HomeController).Remove("Controller"));
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(HomeController).Remove("Controller"));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // METHODS
        [NonAction]
        private async Task Authenticate(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}

using DataAccess.Enums;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

using Apartments_io.Areas.Administrator.ViewModels.Users;

using System.Threading.Tasks;

namespace Apartments_io.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Attributes.Roles(nameof(Role.Administrator))]
    public class UsersController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        // FIELDS
        readonly IUnitOfWork unitOfWork;
        readonly IUserRepository userRepository;

        // CONSTRUCTORS
        public UsersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            ViewData["Title"] = "Administrator";

            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Users = userRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE),
                Managers = userRepository.GetUserByRole(Role.Manager),
                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(userRepository.Count())
            };

            return View(indexViewModel);
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> Create(User user, int managerId)
        {
            if (!userRepository.IsEmailFree(user.Email)) return BadRequest("Email has already been taken");

            user.Manager = await userRepository.GetAsync(managerId);

            await userRepository.InsertAsync(user);
            await unitOfWork.SaveAsync();

            return Ok();
        }
        // ajax
        [HttpPost]
        public async Task<IActionResult> Update(User user, int managerId)
        {
            user.Manager = userRepository.Get(managerId);
            
            unitOfWork.Update(user);
            await unitOfWork.SaveAsync();

            return Ok();
        }
        // ajax
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await userRepository.GetAsync(id);

            if (userRepository.DoesManagerHasAnyResident(user)) return BadRequest("You can not delete manager with renters");
            if (user.Role != Role.Resident && userRepository.IsLastIn(user.Role)) return BadRequest($"You can not delete last {user.Role}");

            userRepository.Delete(user);
            await unitOfWork.SaveAsync();
            
            return Ok();
        }
    }
}
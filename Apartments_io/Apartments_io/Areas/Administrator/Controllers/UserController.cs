using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

using Apartments_io.Areas.Administrator.ViewModels;

using System.Threading.Tasks;

namespace Apartments_io.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class UserController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        // FIELDS
        readonly IUnitOfWork unitOfWork;
        readonly IUserRepository userRepository;

        // CONSTRUCTORS
        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            ViewData["Title"] = "Administrator";

            IndexViewModel indexViewModel = new IndexViewModel();
            
            indexViewModel.Users = userRepository.Get(page, ITEM_PER_PAGE_SIZE);
            indexViewModel.Managers = userRepository.GetUserByRole(DataAccess.Enums.Role.Manager);
            indexViewModel.PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(userRepository.Count());

            return View(indexViewModel);
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> Create(User user, int managerId)
        {
            user.Manager = await userRepository.GetAsync(managerId);

            userRepository.InsertAsync(user);
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

            userRepository.Delete(user);
            await unitOfWork.SaveAsync();
            
            return Ok();
        }
    }
}
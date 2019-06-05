using Apartments_io.Attributes;
using Apartments_io.Areas.Manager.ViewModels.Users;

using Microsoft.AspNetCore.Mvc;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Core.Extensions;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Roles(nameof(DataAccess.Enums.Role.Manager))]
    public class UsersController : Controller
    {
        // CONST
        private static readonly int ITEM_PER_PAGE_SIZE = 6;

        // FIELDS
        IUserRepository userRepository;

        // CONSTRUCTORS
        public UsersController(IUnitOfWork unitOfWork)
        {
            ViewData["Title"] = "Users";
            
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            int managerId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            IndexViewModel viewModel = new IndexViewModel
            {
                UsersStatistic = userRepository.GetUserStatistics(managerId),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(userRepository.Count(r => r.Manager.Id == managerId))
            };

            return View(viewModel);
        }
    }
}
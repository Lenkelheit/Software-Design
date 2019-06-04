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
            ViewData["Title"] = "Administrator";

            this.unitOfWork = unitOfWork;
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            IndexViewModel indexViewModel = new IndexViewModel()
            {
                Users = userRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(userRepository.Count())
            };

            return View(indexViewModel);
        }

        public IActionResult Managers(int page = 1)
        {
            ManagersViewModel managersViewModel = new ManagersViewModel()
            {
                Users = userRepository.Get(filter: u => u.Role == Role.Resident, page: page, amount: ITEM_PER_PAGE_SIZE),

                Managers = userRepository.GetUserByRole(Role.Manager),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(userRepository.Count(u => u.Role == Role.Resident))
            };

            return View(managersViewModel);
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (!userRepository.IsEmailFree(user.Email)) return BadRequest("Email has already been taken");
            
            await userRepository.InsertAsync(user);
            await unitOfWork.SaveAsync();

            return Ok();
        }
        // ajax
        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            if (!ModelState.IsValid) return BadRequest("Model is not valid");

            // get user from DB
            User userDb = await userRepository.GetAsync(user.Id, nameof(DataAccess.Entities.User.Apartments));
            
            if ((userDb.Role == Role.Administrator || userDb.Role == Role.Manager) && userRepository.IsLastIn(userDb.Role))
            {
                return BadRequest($"You can not update role of last {user.Role}");
            }
            if (userDb.Role == Role.Manager && userRepository.DoesManagerHasAnyResident(userDb))
            {
                return BadRequest("You can not update role of manager with renters");
            }

            // update user
            MapUser(user, userDb);
            
            // deactivated users lose all data
            if (user.Role == Role.Deactivated)
            {
                foreach (Apartment apartment in userDb.Apartments)
                {
                    apartment.RentEndDate = null;
                    apartment.Renter = null;
                    apartment.HasUserBeenNotified = null;
                }

                unitOfWork.GetRepository<Bill, BillRepository>()
                    .Delete(b => b.PaymentStatus == PaymentStatus.WaitingForPayment && b.Renter.Id == user.Id);

                unitOfWork.GetRepository<Request, RequestRepository>()
                    .Delete(r => r.Resident.Id == user.Id);

                unitOfWork.GetRepository<Notification, NotificationRepository>()
                    .Delete(n => n.Resident.Id == user.Id);

                userDb.ManagerId = null;
            }

            // update user
            unitOfWork.Update(userDb);
            await unitOfWork.SaveAsync();

            return Ok();
        }
        private void MapUser(User userUpdateData, User userFromDb)
        {
            userFromDb.FirstName = userUpdateData.FirstName;
            userFromDb.LastName = userUpdateData.LastName;
            userFromDb.Password = userUpdateData.Password;
            userFromDb.Email = userUpdateData.Email;
            userFromDb.Role = userUpdateData.Role;
        }
        // ajax
        [HttpPost]
        public async Task<IActionResult> UpdateManager(int userId, int managerId)
        {
            // get user
            User resident = await userRepository.GetAsync(userId);
            if (resident == null) return BadRequest("There is no user with such id");

            // get manager
            User manager = await userRepository.GetAsync(managerId);
            if (manager == null) return BadRequest("There is no manager with such id");

            // set manager
            resident.Manager = manager;

            // update
            unitOfWork.Update(resident);
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
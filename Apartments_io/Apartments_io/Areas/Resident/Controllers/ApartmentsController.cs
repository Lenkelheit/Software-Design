using Microsoft.AspNetCore.Mvc;

using Apartments_io.Attributes;
using Apartments_io.Areas.Resident.ViewModels.Apartments;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Core.Extensions;
using System.Linq;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
    [Roles(nameof(DataAccess.Enums.Role.Resident))]
    public class ApartmentsController : Controller
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        IApartmentRepository apartmentRepository;

        // CONSTRUCTORS
        public ApartmentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.apartmentRepository = unitOfWork.GetRepository<Apartment, ApartmentRepository>();
        }

        // ACTIONS
        public IActionResult Index()
        {
            int RANDOM_APARTMENT_AMOUNT = 2;

            return View(apartmentRepository
                        .GetRandom(amount: RANDOM_APARTMENT_AMOUNT));
        }
        #region LIST
        public IActionResult List(int? minPrice, int? maxPrice, int page = 1)
        {
            int ITEM_PER_PAGE_SIZE = 5;

            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            // count free apartment
            int totalAmount = apartmentRepository.Count(BuildFilter(minPrice, maxPrice));

            // get free apartment
            System.Collections.Generic.IEnumerable<Apartment> apartments =
                apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: BuildFilter(minPrice, maxPrice));

            // save previous filter inputs value
            ViewData[nameof(minPrice)] = minPrice ?? 100;
            ViewData[nameof(maxPrice)] = maxPrice ?? 1000;

            ListViewModel listViewModel = new ListViewModel()
            {
                UserId = loggedUserId,                
                Apartments = apartments,
                IsUsersRequest = apartmentRepository.HasRequests(loggedUserId, apartments.Select(a => a.Id).ToArray()),
                PaginationModel = BuildPagination(ITEM_PER_PAGE_SIZE,page, totalAmount, minPrice, maxPrice)
            };

            return View(listViewModel);
        }
        private System.Linq.Expressions.Expression<System.Func<Apartment, bool>> BuildFilter(int? minPrice, int? maxPrice)
        {
            if (minPrice.HasValue && maxPrice.HasValue) return a => a.Renter == null && a.Price >= minPrice && a.Price < maxPrice;
            else if (minPrice.HasValue) return a => a.Renter == null && a.Price >= minPrice;
            else if (maxPrice.HasValue) return a => a.Renter == null && a.Price < maxPrice;
            else return a => a.Renter == null;
        }
        private Pagination.Pagination BuildPagination(int maxItems, int currentPage, int totalAmount, int? minPrice, int? maxPrice)
        {
            Pagination.Pagination.PaginationFluentBuilder paginationBuilder =
                                            Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(maxItems)
                                                .SetCurrentPage(currentPage)
                                                .SetTotalRecordsAmount(totalAmount);

            // ! adds url fragments 
            if (minPrice.HasValue) paginationBuilder.AddFragment(nameof(minPrice), minPrice.Value);
            if (maxPrice.HasValue) paginationBuilder.AddFragment(nameof(maxPrice), maxPrice.Value);

            return paginationBuilder.Build();
        }
        #endregion
        public async System.Threading.Tasks.Task<IActionResult> CheckApartmentState()
        {
            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            User resident = await unitOfWork.GetRepository<User, UserRepository>().GetAsync(loggedUserId);

            foreach (int apartmentId in apartmentRepository.ExpiredApartment(loggedUserId, daysToExpire: 90))
            {
                // update apartment
                Apartment apartment = await apartmentRepository.GetAsync(apartmentId);
                apartment.HasUserBeenNotified = true;

                unitOfWork.Update(apartment);

                // create notification
                await unitOfWork.GetRepository<Notification, NotificationRepository>()
                        .InsertAsync(new Notification()
                        {
                            EmergencyLevel = DataAccess.Enums.EmergencyLevel.Danger,
                            Resident = resident,
                            Description = "Your rent will expire soon"                            
                        });
            }

            await unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult MyRent(int page = 1)
        {
            int ITEM_PER_PAGE_SIZE = 6;

            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));
            
            ListViewModel listViewModel = new ListViewModel()
            {
                Apartments = apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: a => a.Renter.Id == loggedUserId),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(apartmentRepository.Count(a => a.Renter.Id == loggedUserId))
            };
            
            return View(listViewModel);
        }
        public async System.Threading.Tasks.Task<IActionResult> Single(int? apartmentId)
        {
            if (apartmentId == null) return NotFound();

            // get apartment
            Apartment apartment = await apartmentRepository.GetAsync(apartmentId.Value);
            if (apartment == null) return NotFound();

            // get current user id
            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            SingleViewModel singleViewModel = new SingleViewModel()
            {
                UserId = loggedUserId,
                Apartment = apartment,
                IsRenter = apartmentRepository.IsRenter(apartmentId.Value, loggedUserId),
                HasUserRequest = apartmentRepository.HasRequest(loggedUserId, apartmentId.Value),
            };

            return View(singleViewModel);
        }

        public async System.Threading.Tasks.Task<IActionResult> ContinueRent(int? apartmentId)
        {
            if (apartmentId == null) return NotFound();

            // get apartment
            Apartment apartment = await apartmentRepository.GetAsync(apartmentId.Value);
            if (apartment == null) return NotFound();

            // update rent date
            apartment.RentEndDate = System.DateTime.Now.AddYears(1);

            unitOfWork.Update(apartment);
            await unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Single), new { apartmentId });
        }
    }
}
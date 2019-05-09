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
            int BEST_APARTMENT_AMOUNT = 2;

            return View(apartmentRepository
                        .GetBest(amount: BEST_APARTMENT_AMOUNT));
        }

        public IActionResult List(int page = 1)
        {
            int ITEM_PER_PAGE_SIZE = 5;

            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            // count free apartment
            int totalAmount = apartmentRepository.Count(a => a.Renter == null);

            // get free apartment
            System.Collections.Generic.IEnumerable<Apartment> apartments =
                apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: a => a.Renter == null);

            ListViewModel listViewModel = new ListViewModel()
            {
                UserId = loggedUserId,
                
                Apartments = apartments,

                IsUsersRequest = apartmentRepository.HasRequests(loggedUserId, apartments.Select(a => a.Id).ToArray()),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(totalAmount)
            };

            return View(listViewModel);
        }
        public IActionResult MyRent(int page = 1)
        {
            int ITEM_PER_PAGE_SIZE = 5;

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
    }
}
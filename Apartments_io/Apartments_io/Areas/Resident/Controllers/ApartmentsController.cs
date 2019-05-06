using Microsoft.AspNetCore.Mvc;

using Apartments_io.Areas.Resident.ViewModels.Apartments;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
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

            // count free apartment
            int totalAmount = apartmentRepository.Count(a => a.Renter == null);

            ListViewModel listViewModel = new ListViewModel()
            {
                // get free apartment
                Apartments = apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: a => a.Renter == null),

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

            // change this
            int totalAmount = apartmentRepository.Count(a => a.Renter == null);

            ListViewModel listViewModel = new ListViewModel()
            {
                // TODO: change id
                Apartments = apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: a => a.Renter.Id == 1),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(totalAmount)
            };
            
            return View(listViewModel);
        }
        public async System.Threading.Tasks.Task<IActionResult> Single(int? apartmentId)
        {
            if (apartmentId == null) return NotFound();

            // get apartment
            Apartment apartment = await apartmentRepository.GetAsync(apartmentId.Value);
            if (apartment == null) return NotFound();

            SingleViewModel singleViewModel = new SingleViewModel()
            {
                Apartment = apartment,
                IsRenter = true // TODO: change this
            };

            return View(singleViewModel);
        }             
    }
}
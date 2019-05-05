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

            int totalAmount = apartmentRepository.Count(a => a.Renter == null);

            ListViewModel listViewModel = new ListViewModel()
            {
                Apartments = apartmentRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: a => a.Renter == null),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(totalAmount)
            };

            return View(listViewModel);
        }
    }
}
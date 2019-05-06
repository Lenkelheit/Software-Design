using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Apartments_io.Areas.Manager.ViewModels.Apartments;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ApartmentsController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        // FIELDS
        readonly IUnitOfWork unitOfWork;
        readonly IRepository<Apartment> apartmentsRepository;

        // CONSTRUCTORS
        public ApartmentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.apartmentsRepository = unitOfWork.GetRepository<Apartment, ApartmentRepository>();
        }

        // ACTIONS
        // GET: Manager/Apartments
        public IActionResult Index(int page = 1)
        {
            ViewData["Title"] = "Apartments";

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Apartments = apartmentsRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(apartmentsRepository.Count())
            };
            return View(indexViewModel);
        }

        // GET: Manager/Apartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            Apartment apartment = await apartmentsRepository.GetAsync(id.Value);

            if (apartment == null) return NotFound();

            return View(apartment);
        }

        // GET: Manager/Apartments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manager/Apartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MainPhoto,Name,Description,RentEndDate,Price,Id")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                apartmentsRepository.Insert(apartment);
                await unitOfWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
        }

        // GET: Manager/Apartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            Apartment apartment = await apartmentsRepository.GetAsync(id.Value);
            if (apartment == null) return NotFound();

            return View(apartment);
        }

        // POST: Manager/Apartments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MainPhoto,Name,Description,RentEndDate,Price,Id")] Apartment apartment)
        {
            if (id != apartment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // update apartment
                unitOfWork.Update(apartment);
                
                await unitOfWork.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(apartment);
        }

        // GET: Manager/Apartments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            Apartment apartment = await apartmentsRepository.GetAsync(id.Value);
            if (apartment == null) return NotFound();

            return View(apartment);
        }

        // POST: Manager/Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            apartmentsRepository.Delete(id);
            await unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // ajax
        [HttpPost]
        public IActionResult GetApartmentsList(int userId)
        {
            return Ok(apartmentsRepository
                        .Get(apartment => apartment.Renter.Id == userId));
        }
    }
}

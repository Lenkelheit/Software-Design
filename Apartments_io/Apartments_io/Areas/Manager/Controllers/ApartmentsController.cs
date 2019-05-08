using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Apartments_io.Attributes;
using Apartments_io.Areas.Manager.ViewModels.Apartments;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Roles(nameof(DataAccess.Enums.Role.Manager),
           nameof(DataAccess.Enums.Role.Administrator))]
    public class ApartmentsController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        readonly string[] IMAGE_DIRECTORIES = new string[] { "uploads", "files", "apartment" };
        readonly string DEFAULT_IMAGE = @"/uploads/files/apartment/noimage.png";

        // FIELDS
        readonly IUnitOfWork unitOfWork;
        readonly IApartmentRepository apartmentsRepository;
        readonly IFileService fileService;

        // CONSTRUCTORS
        public ApartmentsController(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.apartmentsRepository = unitOfWork.GetRepository<Apartment, ApartmentRepository>();
            this.fileService = fileService;
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
        public async Task<IActionResult> Create([Bind("Name,Description,RentEndDate,Price,Id")] Apartment apartment, IFormFile uploadFile)
        {
            if (ModelState.IsValid)
            {
                // upload image
                apartment.MainPhoto = await fileService.UploadFileAsync(uploadFile, serverDirectories: IMAGE_DIRECTORIES);
                if (string.IsNullOrWhiteSpace(apartment.MainPhoto)) apartment.MainPhoto = DEFAULT_IMAGE;

                // insert apartment
                await apartmentsRepository.InsertAsync(apartment);
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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,RentEndDate,Price,Id")] Apartment apartment, IFormFile uploadFile)
        {
            if (id != apartment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                apartment.MainPhoto = apartmentsRepository.GetImageById(id);

                // update photo
                if (uploadFile != null)
                {
                    DeleteImageIfNotDefault(apartment);
                    apartment.MainPhoto = await fileService.UploadFileAsync(uploadFile, serverDirectories: IMAGE_DIRECTORIES);
                }

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
            // get apartment
            Apartment apartment = await apartmentsRepository.GetAsync(id);

            // delete image
            DeleteImageIfNotDefault(apartment);

            // delete apartment
            apartmentsRepository.Delete(apartment);
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
        // ajax
        [HttpPost]
        public IActionResult GetApartmentImage(int apartmentId)
        {
            return Ok(apartmentsRepository.GetImageById(apartmentId));
        }

        // METHODS
        private void DeleteImageIfNotDefault(Apartment apartment)
        {
            if (apartment.MainPhoto != DEFAULT_IMAGE)
            {
                fileService.DeleteFile(System.IO.Path.GetFileName(apartment.MainPhoto), IMAGE_DIRECTORIES);
            }
        }
    }
}

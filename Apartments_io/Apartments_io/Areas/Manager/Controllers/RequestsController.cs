using Microsoft.AspNetCore.Mvc;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using System.Linq;
using System.Threading.Tasks;

using Apartments_io.Areas.Manager.ViewModels.Requests;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class RequestsController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        // FIELDS
        IUnitOfWork unitOfWork;
        IRequestRepository requestRepository;

        // CONSTRUCTORS
        public RequestsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.requestRepository = unitOfWork.GetRepository<Request, RequestRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            ViewData["Title"] = "Requests";

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Requests = requestRepository.GetShortInfo(page, ITEM_PER_PAGE_SIZE),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(requestRepository.Count())
            };
            return View(indexViewModel);
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            // get request
            Request request = requestRepository.Get(
                    filter: r => r.Id == requestId,
                    includeProperties: string.Join(',', nameof(request.Apartment), nameof(request.Resident)))
                ?.FirstOrDefault();
            if (request == null) return BadRequest("No request found");

            // accept request
            request.Apartment.Renter = request.Resident;

            // delete request
            requestRepository.Delete(request);

            // save changes
            await unitOfWork.SaveAsync();

            return Ok();
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> DismissRequest(int requestId)
        {
            requestRepository.Delete(requestId);
            await unitOfWork.SaveAsync();

            return Ok();
        }
    }
}

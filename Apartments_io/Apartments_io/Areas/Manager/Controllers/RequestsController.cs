using Core.Extensions;

using Microsoft.AspNetCore.Mvc;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using System.Linq;
using System.Threading.Tasks;

using Apartments_io.Attributes;
using Apartments_io.Areas.Manager.ViewModels.Requests;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Roles(nameof(DataAccess.Enums.Role.Manager))]
    public class RequestsController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;
        readonly int APARTMENT_RENT_PERIOD_IN_YEAR = 1;

        // FIELDS
        IUnitOfWork unitOfWork;
        IRequestRepository requestRepository;

        // CONSTRUCTORS
        public RequestsController(IUnitOfWork unitOfWork)
        {
            ViewData["Title"] = "Requests";

            this.unitOfWork = unitOfWork;
            this.requestRepository = unitOfWork.GetRepository<Request, RequestRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            int managerId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Requests = requestRepository.GetShortInfo(managerId, page, ITEM_PER_PAGE_SIZE),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(requestRepository.Count(r => r.Resident.Manager.Id == managerId))
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
            request.Apartment.RentEndDate = System.DateTime.Now.AddYears(APARTMENT_RENT_PERIOD_IN_YEAR);

            // delete request, and all requests for this apartment
            requestRepository.Delete(r => r.Apartment.Id == request.Apartment.Id);

            // create notification for user
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(new Notification()
            {
                Resident = request.Resident,
                EmergencyLevel = DataAccess.Enums.EmergencyLevel.Success,
                Description = "Your request has been accepted"
            });

            // save changes
            await unitOfWork.SaveAsync();

            return Ok();
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> DismissRequest(int requestId)
        {
            // get request
            Request request = await requestRepository.GetAsync(requestId, nameof(DataAccess.Entities.Request.Resident));
            if (request == null) return BadRequest("No request found");
            
            // delete request
            requestRepository.Delete(request);

            // create notification for user
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(new Notification()
            {
                Resident = requestRepository.Get(requestId).Resident,
                EmergencyLevel = DataAccess.Enums.EmergencyLevel.Warning,
                Description = "Your request has been dismissed"
            });

            // save changes
            await unitOfWork.SaveAsync();

            return Ok();
        }
    }
}

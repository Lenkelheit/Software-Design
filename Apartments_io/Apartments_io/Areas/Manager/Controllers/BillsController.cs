using Apartments_io.Areas.Manager.ViewModels.Bills;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class BillsController : Controller
    {
        // CONST
        readonly int ITEM_PER_PAGE_SIZE = 10;

        // FIELDS
        IUnitOfWork unitOfWork;
        IRepository<Bill> billsRepositories;
        IUserRepository userRepository;

        // CONSTRUCTORS
        public BillsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.billsRepositories = unitOfWork.GetRepository<Bill, GenericRepository<Bill>>();
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        public IActionResult Index(int page = 1)
        {
            ViewData["Title"] = "Bills";

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Bills = billsRepositories.Get(page: page, amount: ITEM_PER_PAGE_SIZE,
                                            includeProperties: string.Join(',', nameof(Bill.Renter), nameof(Bill.Apartment))),

                Renters = userRepository.Get(u => u.Apartments.Count > 0),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(billsRepositories.Count())
            };

            return View(indexViewModel);
        }

        // ajax
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> CreateNewBill(int residentId, int apartmentId)
        {
            // get renter
            User renter = await unitOfWork.GetRepository<User, UserRepository>().GetAsync(residentId);

            // create bill
            Bill bill = new Bill
            {
                Apartment = await unitOfWork.GetRepository<Apartment, ApartmentRepository>().GetAsync(apartmentId),
                Renter = renter,
                PaymentStatus = DataAccess.Enums.PaymentStatus.WaitingForPayment
            };

            // create notifications
            Notification notification = new Notification()
            {
                Resident = renter,
                EmergencyLevel = DataAccess.Enums.EmergencyLevel.Info,
                Description = "You has new bill"
            };
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(notification);

            // save bill
            await billsRepositories.InsertAsync(bill);
            await unitOfWork.SaveAsync();

            return Ok();
        }

        // ajax
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> UpdateBill(int billId, DataAccess.Enums.PaymentStatus paymentStatus)
        {
            // get bill
            Bill bill = await billsRepositories.GetAsync(billId, nameof(Bill.Renter));
            if (bill == null) return BadRequest();

            // update bill
            bill.PaymentStatus = paymentStatus;
            unitOfWork.Update(bill);

            // create notification
            Notification notification = new Notification()
            {
                Resident = bill.Renter,
                EmergencyLevel = DataAccess.Enums.EmergencyLevel.Warning,
                Description = "Your bill has new status " + bill.PaymentStatus
            };
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(notification);

            // save
            await unitOfWork.SaveAsync();

            return Ok();
        }
    }
}
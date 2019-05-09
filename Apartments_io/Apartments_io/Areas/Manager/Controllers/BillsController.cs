using Apartments_io.Attributes;
using Apartments_io.Areas.Manager.ViewModels.Bills;

using DataAccess.Enums;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Roles(nameof(Role.Manager))]
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
        #region INDEX
        public IActionResult Index(int? filterResidentId, PaymentStatus? filterBillStatus,  int page = 1)
        {
            ViewData["Title"] = "Bills";

            // count
            int total = billsRepositories.Count(BuildFilter(filterResidentId, filterBillStatus));

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Bills = billsRepositories.Get(page: page, amount: ITEM_PER_PAGE_SIZE,
                                            includeProperties: string.Join(',', nameof(Bill.Renter), nameof(Bill.Apartment)),
                                            filter: BuildFilter(filterResidentId, filterBillStatus)),

                Renters = userRepository.Get(u => u.Apartments.Count > 0),

                TotalRecordsAmount = total,

                PaginationModel = BuildPagination(ITEM_PER_PAGE_SIZE, page, total, filterResidentId, filterBillStatus)
            };

            return View(indexViewModel);
        }

        private System.Linq.Expressions.Expression<System.Func<Bill, bool>> BuildFilter(int? filterResidentId, PaymentStatus? filterBillStatus)
        {
            if (filterBillStatus.HasValue && filterResidentId.HasValue) return b => b.PaymentStatus == filterBillStatus && b.Renter.Id == filterResidentId;
            else if (filterResidentId.HasValue) return b => b.Renter.Id == filterResidentId;
            else if (filterBillStatus.HasValue) return b => b.PaymentStatus == filterBillStatus;
            else return a => true;
        }
        private Pagination.Pagination BuildPagination(int maxItems, int currentPage, int totalAmount, int? filterResidentId, PaymentStatus? filterBillStatus)
        {
            Pagination.Pagination.PaginationFluentBuilder paginationBuilder =
                                            Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(maxItems)
                                                .SetCurrentPage(currentPage)
                                                .SetTotalRecordsAmount(totalAmount);

            // ! adds url fragments 
            if (filterResidentId.HasValue) paginationBuilder.AddFragment(nameof(filterResidentId), filterResidentId.Value);
            if (filterBillStatus.HasValue) paginationBuilder.AddFragment(nameof(filterBillStatus), filterBillStatus.Value);

            return paginationBuilder.Build();
        }
        #endregion

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
                PaymentStatus = PaymentStatus.WaitingForPayment
            };

            // create notifications
            Notification notification = new Notification()
            {
                Resident = renter,
                EmergencyLevel = EmergencyLevel.Info,
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
        public async System.Threading.Tasks.Task<IActionResult> UpdateBill(int billId, PaymentStatus paymentStatus)
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
                EmergencyLevel = EmergencyLevel.Warning,
                Description = "Your bill has new status " + bill.PaymentStatus
            };
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(notification);

            // save
            await unitOfWork.SaveAsync();

            return Ok();
        }
    }
}
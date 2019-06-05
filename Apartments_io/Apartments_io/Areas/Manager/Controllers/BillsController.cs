using Apartments_io.Attributes;
using Apartments_io.Areas.Manager.ViewModels.Bills;

using DataAccess.Enums;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

using System.Linq;

using Core.Extensions;

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
            ViewData["Title"] = "Bills";

            this.unitOfWork = unitOfWork;
            this.billsRepositories = unitOfWork.GetRepository<Bill, GenericRepository<Bill>>();
            this.userRepository = unitOfWork.GetRepository<User, UserRepository>();
        }

        // ACTIONS
        #region INDEX
        public IActionResult Index(int? filterResidentId, PaymentStatus? filterBillStatus,  int page = 1)
        {
            int managerId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            // count
            int total = billsRepositories.Count(BuildFilter(filterResidentId, filterBillStatus));

            IndexViewModel indexViewModel = new IndexViewModel
            {
                Bills = billsRepositories.Get(page: page, amount: ITEM_PER_PAGE_SIZE,
                                            includeProperties: string.Join(',', nameof(Bill.Renter), nameof(Bill.Apartment)),
                                            filter: BuildFilter(filterResidentId, filterBillStatus),
                                            orderBy: q => q.OrderByDescending(b => b.Id).ThenBy(b => b.PaymentStatus)),

                Renters = userRepository.Get(u => u.Apartments.Count > 0 && u.Manager.Id == managerId),

                TotalRecordsAmount = total,

                PaginationModel = BuildPagination(ITEM_PER_PAGE_SIZE, page, total, filterResidentId, filterBillStatus)
            };

            return View(indexViewModel);
        }

        private System.Linq.Expressions.Expression<System.Func<Bill, bool>> BuildFilter(int? filterResidentId, PaymentStatus? filterBillStatus)
        {
            int managerId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            if (filterBillStatus.HasValue && filterResidentId.HasValue) return b => b.PaymentStatus == filterBillStatus && b.Renter.Id == filterResidentId && b.Renter.Manager.Id == managerId;
            else if (filterResidentId.HasValue) return b => b.Renter.Id == filterResidentId && b.Renter.Manager.Id == managerId;
            else if (filterBillStatus.HasValue) return b => b.PaymentStatus == filterBillStatus && b.Renter.Manager.Id == managerId;
            else return b => b.Renter.Manager.Id == managerId;
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
        public async System.Threading.Tasks.Task<IActionResult> CreateNewBill(int residentId, int apartmentId, System.DateTime billDate)
        {
            // get renter
            User renter = await userRepository.GetAsync(residentId);

            // create bill
            Bill bill = new Bill
            {
                Apartment = await unitOfWork.GetRepository<Apartment, ApartmentRepository>().GetAsync(apartmentId),
                Renter = renter,
                PaymentStatus = PaymentStatus.WaitingForPayment,
                EndDate = billDate
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
        public async System.Threading.Tasks.Task<IActionResult> UpdateBill(int billId, PaymentStatus paymentStatus, System.DateTime billDate)
        {
            // get bill
            Bill bill = await billsRepositories.GetAsync(billId, string.Join(',', nameof(Bill.Renter), nameof(Bill.Apartment)));
            if (bill == null) return BadRequest();

            // get renter
            User renter = bill.Renter;

            // update bill
            bill.PaymentStatus = paymentStatus;
            bill.EndDate = billDate;
            unitOfWork.Update(bill);

            // create notification
            Notification notification = CreateNotification(paymentStatus, bill.Renter);
            await unitOfWork.GetRepository<Notification, GenericRepository<Notification>>().InsertAsync(notification);

            // if overdue..
            if (paymentStatus == PaymentStatus.Overdue)
            {
                // delete all unpaid bills for this apartment
                billsRepositories.Delete(b =>
                    b.Apartment.Id == bill.Apartment.Id &&
                    b.Renter.Id == renter.Id &&
                    b.PaymentStatus == PaymentStatus.WaitingForPayment &&
                    b.Id != bill.Id);

                // delete apartment
                renter.Apartments = null;
                unitOfWork.Update(renter);

                // save changes
                await unitOfWork.SaveAsync();

                // send redirect
                return Ok(new { IsRedirect = true, Location = Url.Action(action: nameof(Index)) }); 
            }

            // save
            await unitOfWork.SaveAsync();

            return Ok();
        }
        private Notification CreateNotification(PaymentStatus paymentStatus, User resident)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.Paid: return new Notification()
                {
                    Resident = resident,
                    EmergencyLevel = EmergencyLevel.Success,
                    Description = "You have paid the bill properly"
                };
                case PaymentStatus.Overdue: return new Notification()
                {
                    Resident = resident,
                    EmergencyLevel = EmergencyLevel.Danger,
                    Description = "You have forgotten to pay. You lose an apartment"
                };
                case PaymentStatus.WaitingForPayment: return new Notification()
                {
                    Resident = resident,
                    EmergencyLevel = EmergencyLevel.Warning,
                    Description = "Your bill has been updated"
                };

                default: throw new System.ArgumentException("Wrong enum type");
            }
        }
    }
}
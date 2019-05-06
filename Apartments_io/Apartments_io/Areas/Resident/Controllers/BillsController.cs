using Microsoft.AspNetCore.Mvc;

using DataAccess.Enums;
using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Apartments_io.Areas.Resident.ViewModels.Bills;

using System.Threading.Tasks;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
    public class BillsController : Controller
    {
        // FIELDS
        IUnitOfWork unitOfWork;
        IBillRepository billRepository;

        // CONSTRUCTORS
        public BillsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.billRepository = unitOfWork.GetRepository<Bill, BillRepository>();
        }

        // ACTIONS
        public IActionResult List()
        {
            ListViewModel listViewModel = new ListViewModel()
            {
                PresentBills = billRepository.GetPresentBills(1), // TODO: change this to real id
                PastBills = billRepository.GetPastBills(1)
            };
            return View(listViewModel);
        }

        // ajax
        [HttpPost]
        public async Task<IActionResult> PayBill(int billId)
        {
            // get bill
            Bill bill = await billRepository.GetAsync(billId, nameof(Bill.Apartment));
            if (bill == null) return BadRequest("There is no such bill");

            // update status
            bill.PaymentStatus = IsPaidInTime(bill) ? PaymentStatus.Paid : PaymentStatus.PaidWithDelay;
            unitOfWork.Update(bill);

            // save
            await unitOfWork.SaveAsync();

            return Ok();

        }
        [NonAction]
        private bool IsPaidInTime(Bill bill)
        {
            return bill.Apartment.RentEndDate >= System.DateTime.Now;
        }
    }
}
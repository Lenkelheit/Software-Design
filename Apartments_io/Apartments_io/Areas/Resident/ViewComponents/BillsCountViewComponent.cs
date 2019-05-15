using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Areas.Resident.ViewComponents
{
    public class BillsCountViewComponent : ViewComponent
    {
        IBillRepository billRepository;

        public BillsCountViewComponent(IUnitOfWork unitOfWork)
        {
            this.billRepository = unitOfWork.GetRepository<Bill, BillRepository>();
        }

        public IViewComponentResult Invoke()
        {
            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            return Content(billRepository.Count(b => b.PaymentStatus == DataAccess.Enums.PaymentStatus.WaitingForPayment && b.Renter.Id == loggedUserId).ToString());
        }
    }
}

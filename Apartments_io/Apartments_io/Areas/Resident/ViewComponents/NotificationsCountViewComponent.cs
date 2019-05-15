using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Apartments_io.Areas.Resident.ViewComponents
{
    public class NotificationsCountViewComponent : ViewComponent
    {
        IRepository<Notification> notificationRepository;

        public NotificationsCountViewComponent(IUnitOfWork unitOfWork)
        {
            this.notificationRepository = unitOfWork.GetRepository<Notification, GenericRepository<Notification>>();
        }

        public IViewComponentResult Invoke()
        {
            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));

            return Content(notificationRepository.Count(n => n.Resident.Id == loggedUserId).ToString());
        }
    }
}

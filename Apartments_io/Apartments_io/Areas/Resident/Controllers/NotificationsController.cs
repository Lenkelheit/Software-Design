﻿using Core.Extensions;

using DataAccess.Entities;
using DataAccess.Interfaces;
using DataAccess.Repositories;

using Microsoft.AspNetCore.Mvc;

using Apartments_io.Attributes;
using Apartments_io.Areas.Resident.ViewModels.Notifications;

using System.Threading.Tasks;

namespace Apartments_io.Areas.Resident.Controllers
{
    [Area("Resident")]
    [Roles(nameof(DataAccess.Enums.Role.Resident),
           nameof(DataAccess.Enums.Role.Manager),
           nameof(DataAccess.Enums.Role.Administrator))]
    public class NotificationsController : Controller
    {
        // CONST
        static readonly int ITEM_PER_PAGE_SIZE = 5;

        // FIELDS
        IUnitOfWork unitOfWork;
        IRepository<Notification> notificationRepository;

        // CONSTRUCTORS
        public NotificationsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.notificationRepository = unitOfWork.GetRepository<Notification, GenericRepository<Notification>>();
        }

        // ACTIONS
        public IActionResult List(int page = 1)
        {
            int loggedUserId = this.GetClaim<int>(nameof(DataAccess.Entities.User.Id));
            
            ListViewModel listViewModel = new ListViewModel()
            {
                Notifications = notificationRepository.Get(page: page, amount: ITEM_PER_PAGE_SIZE, filter: n => n.Resident.Id == loggedUserId),

                PaginationModel = Pagination.Pagination.GetBuilder
                                                .SetRecordsAmountPerPage(ITEM_PER_PAGE_SIZE)
                                                .SetCurrentPage(page)
                                                .SetTotalRecordsAmount(notificationRepository.Count(n => n.Resident.Id == loggedUserId))
            };

            return View(listViewModel);
        }
        // ajax
        [HttpPost]
        public async Task<IActionResult> ConfirmNotification(int notificationId)
        {
            // delete notification
            notificationRepository.Delete(notificationId);
            await unitOfWork.SaveAsync();

            return Ok();

        }
    }
}
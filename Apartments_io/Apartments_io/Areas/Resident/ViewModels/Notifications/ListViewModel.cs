using System.Collections.Generic;

using DataAccess.Entities;


namespace Apartments_io.Areas.Resident.ViewModels.Notifications
{
    public class ListViewModel
    {
        public IEnumerable<Notification> Notifications { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

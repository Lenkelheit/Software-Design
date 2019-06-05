using DataAccess.Entities;

using System.Collections.Generic;

namespace Apartments_io.Areas.Administrator.ViewModels.Users
{
    public class ManagersViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<User> Managers { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

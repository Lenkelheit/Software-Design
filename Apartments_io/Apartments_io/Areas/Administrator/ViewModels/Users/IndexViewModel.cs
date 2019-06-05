using System.Collections.Generic;

using DataAccess.Entities;

namespace Apartments_io.Areas.Administrator.ViewModels.Users
{
    public class IndexViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

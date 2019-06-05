using DataAccess.Entities;

using System.Collections.Generic;

namespace Apartments_io.Areas.Manager.ViewModels.Bills
{
    public class IndexViewModel
    {
        public IEnumerable<Bill> Bills { get; set; }
        public IEnumerable<User> Renters { get; set; }
        public int TotalRecordsAmount { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

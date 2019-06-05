using DataAccess.Entities;

using System.Collections.Generic;

namespace Apartments_io.Areas.Manager.ViewModels.Requests
{
    public class IndexViewModel
    {
        public IEnumerable<Request> Requests { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

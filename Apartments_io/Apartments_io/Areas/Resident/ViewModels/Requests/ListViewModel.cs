using System.Collections.Generic;

using DataAccess.Entities;

namespace Apartments_io.Areas.Resident.ViewModels.Requests
{
    public class ListViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<Request> Requests { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

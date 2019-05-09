using System.Collections.Generic;

using DataAccess.Entities;

namespace Apartments_io.Areas.Resident.ViewModels.Apartments
{
    public class ListViewModel
    {
        public int UserId { get; set; }
        public IEnumerable<Apartment> Apartments { get; set; }
        public ISet<int> IsUsersRequest { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

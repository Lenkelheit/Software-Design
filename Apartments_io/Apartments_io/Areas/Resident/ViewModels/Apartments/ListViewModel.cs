using System.Collections.Generic;

using DataAccess.Entities;

namespace Apartments_io.Areas.Resident.ViewModels.Apartments
{
    public class ListViewModel
    {
        public IEnumerable<Apartment> Apartments { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

using DataAccess.Entities;

using System.Collections.Generic;

namespace Apartments_io.Areas.Manager.ViewModels.Apartments
{
    public class IndexViewModel
    {
        public IEnumerable<Apartment> Apartments { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

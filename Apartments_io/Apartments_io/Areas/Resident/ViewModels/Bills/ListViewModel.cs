using System.Collections.Generic;

using DataAccess.Entities;

namespace Apartments_io.Areas.Resident.ViewModels.Bills
{
    public class ListViewModel
    {
        public IEnumerable<Bill> PresentBills { get; set; }
        public IEnumerable<Bill> PastBills { get; set; }
    }
}

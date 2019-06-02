namespace Apartments_io.Areas.Manager.ViewModels.Users
{
    public class IndexViewModel
    {
        public System.Collections.Generic.IEnumerable<DataAccess.Wrappers.UserStatisticWrapper> UsersStatistic { get; set; }
        public Pagination.Pagination PaginationModel { get; set; }
    }
}

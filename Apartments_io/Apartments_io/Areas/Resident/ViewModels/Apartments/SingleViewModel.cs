namespace Apartments_io.Areas.Resident.ViewModels.Apartments
{
    public class SingleViewModel
    {
        public int UserId { get; set; }
        public DataAccess.Entities.Apartment Apartment { get; set; }
        public bool IsRenter { get; set; }
        public bool HasUserRequest { get; set; }
    }
}

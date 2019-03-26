namespace DataAccess.Entities
{
    public class Request
    {
        public virtual User Renter { get; set; }
        public virtual Apartment Apartment { get; set; }
    }
}

namespace DataAccess.Entities
{
    public class Notification : EntityBase
    {
        public string Description { get; set; }
        public virtual User Renter { get; set; }
    }
}

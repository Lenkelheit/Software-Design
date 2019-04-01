using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Bill : EntityBase
    {
        public Enums.PaymentStatus PaymentStatus { get; set; }
        public virtual User Renter { get; set; }
        public virtual Apartment Apartment { get; set; }
    }
}

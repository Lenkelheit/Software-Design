using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class Apartment : EntityBase
    {
        public string MainPhoto { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime RentEndDate { get; set; }
        public float Price { get; set; }
        public virtual User Renter { get; set; }
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}

using System.Collections.Generic;

namespace DataAccess.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Enums.Role Role { get; set; }
        public virtual User Manager { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}

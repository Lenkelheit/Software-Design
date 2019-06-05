using System.Collections.Generic;

namespace DataAccess.Entities
{
    /// <summary>
    /// Maps to User table
    /// </summary>
    public class User : EntityBase
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// User's last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// User's role
        /// </summary>
        public Enums.Role Role { get; set; }
        /// <summary>
        /// Manager's id
        /// </summary>
        public int? ManagerId { get; set; }
        /// <summary>
        /// Resident's manager
        /// </summary>
        public virtual User Manager { get; set; }

        /// <summary>
        /// A list of manager's residents
        /// </summary>
        public virtual ICollection<User> Resident { get; set; } = new List<User>();
        /// <summary>
        /// A list of user's apartments
        /// </summary>
        public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
        /// <summary>
        /// A list of user's requests
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        /// <summary>
        /// A list of user's unread notifications
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        /// <summary>
        /// A list of user's bills
        /// </summary>
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();


        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>
        /// A string that represents the current object
        /// </returns>
        public override string ToString()
        {
            return string.Join(' ', FirstName, LastName);
        }
    }
}

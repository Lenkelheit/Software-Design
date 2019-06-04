using System.Collections.Generic;

namespace DataAccess.Entities
{
    /// <summary>
    /// Maps to Apartment table
    /// </summary>
    public class Apartment : EntityBase
    {
        /// <summary>
        /// Main photo path
        /// </summary>
        public string MainPhoto { get; set; }
        /// <summary>
        /// Name of apartment
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Apartment's description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Last date of rent
        /// </summary>
        public System.DateTime? RentEndDate { get; set; }
        /// <summary>
        /// Apartment's price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Renter
        /// </summary>
        public virtual User Renter { get; set; }
        /// <summary>
        /// Determines if user has get notification about his apartment expiration date
        /// </summary>
        public bool? HasUserBeenNotified { get; set; }
        /// <summary>
        /// A list of requests for current apartment
        /// </summary>
        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        /// <summary>
        /// A history of apartment's bills
        /// </summary>
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}

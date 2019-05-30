namespace DataAccess.Entities
{
    /// <summary>
    /// Maps to Bill table
    /// </summary>
    public class Bill : EntityBase
    {
        /// <summary>
        /// A payment status of a bill
        /// </summary>
        public Enums.PaymentStatus PaymentStatus { get; set; }
        /// <summary>
        /// Gets or sets date to paid bill properly
        /// </summary>
        public System.DateTime EndDate { get; set; }
        /// <summary>
        /// A resident, that paid for apartment
        /// </summary>
        public virtual User Renter { get; set; }
        /// <summary>
        /// An apartment for which resident paid
        /// </summary>
        public virtual Apartment Apartment { get; set; }
    }
}

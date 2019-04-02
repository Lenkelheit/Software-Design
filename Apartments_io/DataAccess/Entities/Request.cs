namespace DataAccess.Entities
{
    /// <summary>
    /// Maps to Request table
    /// </summary>
    public class Request : EntityBase
    {
        /// <summary>
        /// A resident who send a request for the apartment
        /// </summary>
        public virtual User Resident { get; set; }
        /// <summary>
        /// An apartment for which resident send the request
        /// </summary>
        public virtual Apartment Apartment { get; set; }
    }
}

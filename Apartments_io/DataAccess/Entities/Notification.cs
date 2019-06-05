namespace DataAccess.Entities
{
    /// <summary>
    /// Maps to Notification table
    /// </summary>
    public class Notification : EntityBase
    {
        /// <summary>
        /// Notification's description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// An resident, who got the notification 
        /// </summary>
        public virtual User Resident { get; set; }
        /// <summary>
        /// Notification's emergency level
        /// </summary>
        public Enums.EmergencyLevel EmergencyLevel { get; set; }
    }
}

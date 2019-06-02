namespace DataAccess.Wrappers
{
    /// <summary>
    /// A wrapper for user with its bill statistic
    /// </summary>
    public class UserStatisticWrapper
    {
        /// <summary>
        /// Gets or sets current user
        /// </summary>
        public Entities.User User { get; set; }
        /// <summary>
        /// Gets or sets bills statistic with waiting for payment status
        /// </summary>
        public Structs.ValuePercentage WaitingForPaymentBills { get; set; }
        /// <summary>
        /// Gets or sets bills statistic with paid properly status
        /// </summary>
        public Structs.ValuePercentage PaidBills { get; set; }
        /// <summary>
        /// Gets or sets bills statistic with paid with delay status
        /// </summary>
        public Structs.ValuePercentage PaidWithDelayBills { get; set; }
        /// <summary>
        /// Gets or sets bills statistic with overdue status
        /// </summary>
        public Structs.ValuePercentage OverdueBills { get; set; }
    }
}

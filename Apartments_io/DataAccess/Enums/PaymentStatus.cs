namespace DataAccess.Enums
{
    /// <summary>
    /// Specify status of the payment
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Is not paid and up to date
        /// </summary>
        WaitingForPayment,
        /// <summary>
        /// Is paid properly
        /// </summary>
        Paid,
        /// <summary>
        /// Not paid
        /// </summary>
        Overdue,
        /// <summary>
        /// Paid, but with delay
        /// </summary>
        PaidWithDelay
    }
}

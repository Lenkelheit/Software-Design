using DataAccess.Entities;

using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for classes which will proxy behind data access and business logic for bill
    /// </summary>
    public interface IBillRepository : IRepository<Bill>
    {
        /// <summary>
        /// Gets current bills for user
        /// </summary>
        /// <param name="userId">
        /// User's id for whom bills should be queried
        /// </param>
        /// <param name="paymentStatus">
        /// Status of bills
        /// </param>
        /// <returns>
        /// Queried bills for current user with current status
        /// </returns>
        IEnumerable<Bill> GetPresentBills(int userId, Enums.PaymentStatus paymentStatus = Enums.PaymentStatus.WaitingForPayment);

        /// <summary>
        /// Gets past bills for user
        /// </summary>
        /// <param name="userId">
        /// User's id for whom bills should be queried
        /// </param>
        /// <param name="paymentStatus">
        /// Status of bills
        /// </param>
        /// <returns>
        /// Queried bills for current user with current status
        /// </returns>
        IEnumerable<Bill> GetPastBills(int userId, Enums.PaymentStatus paymentStatus = Enums.PaymentStatus.WaitingForPayment);
    }
}

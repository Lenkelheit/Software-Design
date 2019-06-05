using System.Linq;
using System.Collections.Generic;

using DataAccess.Enums;
using DataAccess.Entities;
using DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic for bill table
    /// </summary>
    public class BillRepository : GenericRepository<Bill>, IBillRepository
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
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<Bill> GetPastBills(int userId, PaymentStatus paymentStatus = PaymentStatus.WaitingForPayment)
        {
            ContextCheck();

            return dbSet
                .Include(nameof(Bill.Apartment))
                .OrderBy(b => b.EndDate)
                .Where(b => b.Renter.Id == userId && b.PaymentStatus == paymentStatus && b.EndDate < System.DateTime.Now);
        }

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
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<Bill> GetPresentBills(int userId, PaymentStatus paymentStatus = PaymentStatus.WaitingForPayment)
        {
            ContextCheck();

            return dbSet
                .Include(nameof(Bill.Apartment))
                .OrderBy(b => b.EndDate)
                .Where(b => b.Renter.Id == userId && b.PaymentStatus == paymentStatus && b.EndDate >= System.DateTime.Now);
        }
    }
}

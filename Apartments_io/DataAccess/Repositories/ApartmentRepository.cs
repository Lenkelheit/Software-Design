using System.Linq;
using System.Collections.Generic;

using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic for apartment table
    /// </summary>
    public class ApartmentRepository : GenericRepository<Apartment>, IApartmentRepository
    {
        /// <summary>
        /// Determines if user rent current apartment
        /// </summary>
        /// <param name="apartmentId">
        /// Apartment's id
        /// </param>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <returns>
        /// True if user is renter, otherwise — false
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public bool IsRenter(int apartmentId, int userId)
        {
            ContextCheck();

            return dbSet.Any(a => a.Id == apartmentId && a.Renter.Id == userId);
        }
        /// <summary>
        /// Return collection of best apartments
        /// </summary>
        /// <param name="amount">
        /// Amount of items to return
        /// </param>
        /// <returns>
        /// Collection of best apartments
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public IEnumerable<Apartment> GetBest(int amount)
        {
            ContextCheck();
            return dbSet
                    .Take(amount)
                    .AsEnumerable();

            /*return (from apartment in dbSet
                    join bill in dbContext.Set<Bill>() on apartment.Id equals bill.Apartment.Id into apartmentsBills
                    from apartmentBills in apartmentsBills.DefaultIfEmpty()
                    group apartmentBills by apartmentBills.Apartment into apartmentGroup
                    select apartmentGroup.Key)
                    .Take(amount)
                    .AsEnumerable();*/
        }
    }
}

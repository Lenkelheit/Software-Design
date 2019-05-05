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

            return (from apartment in dbSet
                    join bill in dbContext.Set<Bill>() on apartment.Id equals bill.Apartment.Id
                    group apartment by apartment into apartmentGroup
                    select apartmentGroup.Key)
                    .Take(amount)
                    .AsEnumerable();
        }
    }
}

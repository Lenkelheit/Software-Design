using System.Linq;
using System.Collections.Generic;

using DataAccess.Entities;
using DataAccess.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic for apartment table
    /// </summary>
    public class ApartmentRepository : GenericRepository<Apartment>, IApartmentRepository
    {
        /// <summary>
        /// Gets image relative path by id
        /// </summary>
        /// <param name="id">
        /// Apartment's id
        /// </param>
        /// <returns>
        /// Relative image path
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual string GetImageById(int id)
        {
            ContextCheck();

            return dbSet.AsNoTracking().First(a => a.Id == id).MainPhoto;
        }
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
        public virtual bool IsRenter(int apartmentId, int userId)
        {
            ContextCheck();

            return dbSet.Any(a => a.Id == apartmentId && a.Renter.Id == userId);
        }
        /// <summary>
        /// Return collection of random free apartments
        /// </summary>
        /// <param name="amount">
        /// Amount of items to return
        /// </param>
        /// <returns>
        /// Collection of random apartments
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<Apartment> GetRandom(int amount)
        {
            ContextCheck();

            System.Random random = new System.Random();

            int[] ids = dbSet.Where(a => a.Renter == null).Select(x => x.Id).ToArray();
            HashSet<int> randomIds = new HashSet<int>(amount);
            while (randomIds.Count != amount && randomIds.Count != ids.Length)
            {
                randomIds.Add(ids[random.Next(ids.Length)]); 
            }

            return dbSet
                    .Where(a => a.Renter == null)
                    .Where(a => randomIds.Contains(a.Id))
                    .Take(amount)
                    .AsEnumerable();
        }


        /// <summary>
        /// Determines if users has sent request for current apartments
        /// </summary>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <param name="apartmentsIds">
        /// Apartments' ids to check
        /// </param>
        /// <returns>
        /// Sets with apartments' ids for which user has sent request
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual ISet<int> HasRequests(int userId, IEnumerable<int> apartmentsIds)
        {
            ContextCheck();

            return dbSet
                    .Where(a => apartmentsIds.Contains(a.Id))
                    .Where(a => a.Requests.Select(r => r.Resident.Id).Contains(userId))
                    .Select(a => a.Id)
                    .ToHashSet();

        }
        /// <summary>
        /// Determines if users has sent request for current apartment
        /// </summary>
        /// <param name="userId">
        /// User's id
        /// </param>
        /// <param name="apartmentId">
        /// Apartments' id
        /// </param>
        /// <returns>
        /// True if user has sent request for current apartment, otherwise — false
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual bool HasRequest(int userId, int apartmentId)
        {
            ContextCheck();

            return dbContext.Set<Request>().Any(r => r.Apartment.Id == apartmentId && r.Resident.Id == userId);
                    
                    
        }
        /// <summary>
        /// Gets ids of apartments for current user that will expire in current period
        /// </summary>
        /// <param name="renterId">
        /// Renter's id
        /// </param>
        /// <param name="daysToExpire">
        /// Expire period for apartments
        /// </param>
        /// <returns>
        /// List with ids of apartments that will expire in current period
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<int> ExpiredApartment(int renterId, int daysToExpire)
        {
            ContextCheck();

            return dbSet
                .Where(a => a.Renter.Id == renterId)
                .Where(a => a.HasUserBeenNotified == null || a.HasUserBeenNotified == false)
                .Where(a => (a.RentEndDate.Value - System.DateTime.Now).Days <= daysToExpire)
                .Select(a => a.Id);
        }
    }
}

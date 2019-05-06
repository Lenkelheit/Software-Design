﻿using System.Linq;
using System.Collections.Generic;

using DataAccess.Enums;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Proxy data access and business logic for user table
    /// </summary>
    public class UserRepository : GenericRepository<User>, Interfaces.IUserRepository
    {
        /// <summary>
        /// Gets collections of user by theirs role
        /// </summary>
        /// <param name="role">
        /// User role
        /// </param>
        /// <returns>
        /// A collection of user of certain role
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual IEnumerable<User> GetUserByRole(Role role)
        {
            ContextCheck();

            return dbSet.Where(user => user.Role == role);
        }

        /// <summary>
        /// Determines if manager has any resident
        /// </summary>
        /// <param name="manager">
        /// manager to check
        /// </param>
        /// <returns>
        /// True if manager has any residents, otherwise — false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Throws when <paramref name="manager"/> is null
        /// </exception>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual bool DoesManagerHasAnyResident(User manager)
        {
            ContextCheck();

            if (manager == null) throw new System.ArgumentNullException(nameof(manager));

            if (manager.Role != Role.Manager) return false;

            return dbSet.Any(resident => resident.Manager.Id == manager.Id);
        }
    }
}

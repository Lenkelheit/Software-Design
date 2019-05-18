using System.Linq;
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
        /// <summary>
        /// Checks if current email is free
        /// </summary>
        /// <param name="email">
        /// Email that should be checked
        /// </param>
        /// <returns>
        /// True if email is free, otherwise false
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Throws when passed <paramref name="email"/> is null
        /// </exception>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public bool IsEmailFree(string email)
        {
            ContextCheck();
            if (string.IsNullOrWhiteSpace(email)) throw new System.ArgumentNullException(nameof(email));

            // return true with First occurrence
            return !dbSet.Any(user => user.Email == email);
        }
        /// <summary>
        /// Checks if current email and password are valid
        /// </summary>
        /// <param name="email">
        /// The email that should be checked
        /// </param>
        /// <param name="password">
        /// The password that should be checked
        /// </param>
        /// <returns>
        /// Returns a structure that defines if Email and Password are valid and by them returns this user 
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// Throws when passed <paramref name="email"/> is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Throws when passed <paramref name="password"/> is null
        /// </exception>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public virtual System.Tuple<bool, bool, User> IsDataValid(string email, string password)
        {
            // checking
            ContextCheck();
            if (string.IsNullOrWhiteSpace(email)) throw new System.ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(password)) throw new System.ArgumentNullException(nameof(password));

            // get first user with such email
            User foundedAccount = dbSet.FirstOrDefault(user => user.Email == email);

            // if no user has been found return bool values as false and user as null
            if (foundedAccount == null) return System.Tuple.Create<bool, bool, User>(false, false, null);

            // returns email as True and Password if the same, user if Password is true
            bool isPasswordRight = foundedAccount.Password == password;
            return System.Tuple.Create<bool, bool, User>(true, isPasswordRight, isPasswordRight ? foundedAccount : null);
        }

        /// <summary>
        /// Checks is user is last in his role
        /// </summary>
        /// <param name="role">
        /// Role to be checked
        /// </param>
        /// <returns>
        /// True if user is last in his role, otherwise — falses
        /// </returns>
        /// <exception cref="System.NullReferenceException">
        /// Throws when context for this repository is not set<para/>
        /// Try to call <see cref="!:SetDbContext(Microsoft.EntityFrameworkCore.Internal.IDbContextDependencies)"/> method
        /// </exception>
        public bool IsLastIn(Role role)
        {
            ContextCheck();

            return dbSet.Count(u => u.Role == role) == 1;
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Core.Extensions
{
    /// <summary>
    /// Contains extensions for <see cref="ViewComponentExtension"/>
    /// </summary>
    public static class ViewComponentExtension
    {
        /// <summary>
        /// Gets claim's value by its type
        /// </summary>
        /// <typeparam name="T">
        /// Type of claim's value
        /// </typeparam>
        /// <param name="viewComponent">
        /// An instance of <see cref="ViewComponentExtension"/> that has been extended
        /// </param>
        /// <param name="claimType">
        /// Claim's type
        /// </param>
        /// <returns>
        /// Claim's value
        /// </returns>
        public static T GetClaim<T>(this ViewComponent viewComponent, string claimType)
        {
            return (T)System.Convert.ChangeType(((System.Security.Claims.ClaimsPrincipal)viewComponent.User).FindFirst(claimType).Value, typeof(T));
        }
    }
}

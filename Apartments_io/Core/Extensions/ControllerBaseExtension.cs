using Microsoft.AspNetCore.Mvc;

namespace Core.Extensions
{
    /// <summary>
    /// Contains extensions for <see cref="ControllerBase"/>
    /// </summary>
    public static class ControllerBaseExtension
    {
        /// <summary>
        /// Redirects to the specified action using the specified areaName, actionName and controllerName
        /// </summary>
        /// <param name="controller">
        /// An instance of <see cref="ControllerBase"/> that has been extended
        /// </param>
        /// <param name="areaName">
        /// Area's name
        /// </param>
        /// <param name="controllerName">
        /// Controller's name
        /// </param>
        /// <param name="actionName">
        /// Action's name
        /// </param>
        /// <returns>
        /// The created <see cref="RedirectToActionResult"/> for the response.
        /// </returns>
        public static RedirectToActionResult RedirectToAction(this ControllerBase controller, string areaName, string controllerName, string actionName)
        {
            return controller.RedirectToAction(actionName, controllerName, new { area = areaName });
        }

        /// <summary>
        /// Gets claim's value by its type
        /// </summary>
        /// <typeparam name="T">
        /// Type of claim's value
        /// </typeparam>
        /// <param name="controller">
        /// An instance of <see cref="ControllerBase"/> that has been extended
        /// </param>
        /// <param name="claimType">
        /// Claim's type
        /// </param>
        /// <returns>
        /// Claim's value
        /// </returns>
        public static T GetClaim<T>(this ControllerBase controller, string claimType) 
        {
            return (T)System.Convert.ChangeType(controller.User.FindFirst(claimType).Value, typeof(T));
        }
    }
}

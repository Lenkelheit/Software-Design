namespace Pagination.DataTransferObject
{
    /// <summary>
    /// Contains data to build URL
    /// </summary>
    public class UrlInfo
    {
        /// <summary>
        /// Contains methods to build URL
        /// </summary>
        public Microsoft.AspNetCore.Mvc.IUrlHelper UrlHelper { get; set; }
        /// <summary>
        /// Determines controller's name
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Determines action's name
        /// </summary>
        public string ActionName { get; set; }
    }
}

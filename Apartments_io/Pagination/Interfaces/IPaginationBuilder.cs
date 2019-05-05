using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.Interfaces
{
    /// <summary>
    /// Determines steps to build pagination
    /// </summary>
    public interface IPaginationBuilder
    {
        /// <summary>
        /// Build body
        /// </summary>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        TagBuilder BuildBody();
        /// <summary>
        /// Build link to another page
        /// </summary>
        /// <param name="page">
        /// Determines to which page link is
        /// </param>
        /// <param name="text">
        /// Determines text on link
        /// </param>
        /// <param name="isActive">
        /// Determines is link active in current momment
        /// </param>
        /// <param name="isDisabled">
        /// Determines is link disabled
        /// </param>
        /// <param name="urlInfo">
        /// Contain data to build URL
        /// </param>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        TagBuilder GenerateLink(int page, string text, bool isActive, bool isDisabled, DataTransferObject.UrlInfo urlInfo);
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.Interfaces
{
    /// <summary>
    /// Determines steps to build pagination <para/>
    /// Implements this if you want to change pagination's view
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
        /// <param name="linkInfo">
        /// Determines parameters for link: text, page, isActive, isDisabled etc
        /// </param>
        /// <param name="urlInfo">
        /// Contain data to build URL
        /// </param>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        TagBuilder GenerateLink(DataTransferObject.LinkInfo linkInfo, DataTransferObject.UrlInfo urlInfo);
    }
}

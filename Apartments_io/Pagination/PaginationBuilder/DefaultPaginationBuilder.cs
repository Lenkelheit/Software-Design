using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.PaginationBuilder
{
    /// <summary>
    /// Determines steps to build pagination with Bootstrap way
    /// </summary>
    public class DefaultPaginationBuilder : Interfaces.IPaginationBuilder
    {
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
        public TagBuilder GenerateLink(int page, string text, bool isActive, bool isDisabled, DataTransferObject.UrlInfo urlInfo)
        {
            // a
            TagBuilder link = new TagBuilder("a");
            link.AddCssClass("page-link");
            if (!isActive)
            {
                link.Attributes["href"] = urlInfo.UrlHelper.Action(
                                            action: urlInfo.ActionName,
                                            controller: urlInfo.ControllerName,
                                            values: new { page = page });
            }            

            link.InnerHtml.Append(text);

            // li
            TagBuilder item = new TagBuilder("li");
            item.AddCssClass("page-item");
            if (isActive) item.AddCssClass("active");
            if (isDisabled) item.AddCssClass("disabled");

            item.InnerHtml.AppendHtml(link);
            return item;
        }

        /// <summary>
        /// Build body
        /// </summary>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        public TagBuilder BuildBody()
        {
            TagBuilder body = new TagBuilder("ul");
            body.AddCssClass("pagination");
            body.AddCssClass("justify-content-center");
            return body;
        }
    }
}

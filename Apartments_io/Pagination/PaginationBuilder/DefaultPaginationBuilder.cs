using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
        /// <param name="linkInfo">
        /// Determines parameters for link: text, page, isActive, isDisabled etc
        /// </param>
        /// <param name="urlInfo">
        /// Contain data to build URL
        /// </param>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        public TagBuilder GenerateLink(DataTransferObject.LinkInfo linkInfo, DataTransferObject.UrlInfo urlInfo)
        {
            // a
            TagBuilder link = new TagBuilder("a");
            link.AddCssClass("page-link");
            if (!linkInfo.IsActive)
            {
                Dictionary<string, object> urlFragments = new Dictionary<string, object>(linkInfo.Fragments);
                urlFragments.Add(linkInfo.PageKey, linkInfo.Page);

                link.Attributes["href"] = urlInfo.UrlHelper.Action(
                                            action: urlInfo.ActionName,
                                            controller: urlInfo.ControllerName,
                                            values: urlFragments);
            }
            else
            {
                link.Attributes["href"] = "#";
            }
            

            link.InnerHtml.Append(linkInfo.Text);

            // li
            TagBuilder item = new TagBuilder("li");
            item.AddCssClass("page-item");
            if (linkInfo.IsActive) item.AddCssClass("active");
            if (linkInfo.IsDisabled) item.AddCssClass("disabled");

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

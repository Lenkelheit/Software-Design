using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.PaginationBuilder
{
    public class DefaultPaginationBuilder : Interfaces.IPaginationBuilder
    {
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

        public TagBuilder BuildBody()
        {
            TagBuilder body = new TagBuilder("ul");
            body.AddCssClass("pagination");
            body.AddCssClass("justify-content-center");
            return body;
        }
    }
}

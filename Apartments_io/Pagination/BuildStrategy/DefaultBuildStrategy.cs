using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pagination.BuildStrategy
{
    public class DefaultBuildStrategy : Interfaces.IBuildStrategy
    {
        public TagBuilder Build(Pagination pagination)
        {
            PaginationLimit limits = pagination.CalcPagintaionLimits();
            Interfaces.IPaginationBuilder builder = pagination.PaginationBuilder;

            TagBuilder body = builder.BuildBody();

            // previous
            body.InnerHtml.AppendHtml(builder.GenerateLink(pagination.CurrentPage - 1, "Previous", false, !pagination.HasPreviousPage, pagination.UrlInfo));

            // middle
            for (int page = limits.StartPage; page <= limits.EndPage; ++page)
            {
                if (page == pagination.CurrentPage) body.InnerHtml.AppendHtml(builder.GenerateLink(page, page.ToString(), true, false, pagination.UrlInfo));
                else body.InnerHtml.AppendHtml(builder.GenerateLink(page, page.ToString(), false, false, pagination.UrlInfo));
            }

            // next           
            body.InnerHtml.AppendHtml(builder.GenerateLink(pagination.CurrentPage + 1, "Next", false, !pagination.HasNextPage, pagination.UrlInfo));

            return body;
        }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

using Pagination.DataTransferObject;

namespace Pagination.BuildStrategy
{
    /// <summary>
    /// Build pagination in default Bootstrap ways
    /// </summary>
    public class DefaultBuildStrategy : Interfaces.IBuildStrategy
    {
        // METHODS
        /// <summary>
        /// Build pagination
        /// </summary>
        /// <param name="pagination">
        /// Pagination model with data
        /// </param>
        /// <returns>
        /// The class that can create HTML elements
        /// </returns>
        public TagBuilder Build(Pagination pagination)
        {
            PaginationLimit limits = pagination.CalcPagintaionLimits();
            Interfaces.IPaginationBuilder builder = pagination.PaginationBuilder;

            TagBuilder body = builder.BuildBody();

            // previous
            body.InnerHtml.AppendHtml(builder.GenerateLink(CreateLinkInfo(LinkInfoType.Previous, pagination, -1), pagination.UrlInfo));

            // middle area
            for (int page = limits.StartPage; page <= limits.EndPage; ++page)
            {
                if (page == pagination.CurrentPage)
                {
                    body.InnerHtml.AppendHtml(builder.GenerateLink(CreateLinkInfo(LinkInfoType.Current, pagination, page), pagination.UrlInfo));
                }
                else
                {
                    body.InnerHtml.AppendHtml(builder.GenerateLink(CreateLinkInfo(LinkInfoType.Regular, pagination, page), pagination.UrlInfo));
                }
            }

            // next
            body.InnerHtml.AppendHtml(builder.GenerateLink(CreateLinkInfo(LinkInfoType.Next, pagination, -1), pagination.UrlInfo));

            return body;
        }

        enum LinkInfoType
        {
            Previous,
            Next,
            Regular,
            Current
        }
        private LinkInfo CreateLinkInfo(LinkInfoType type, Pagination pagination, int page)
        {
            switch (type)
            {
                case LinkInfoType.Previous: return new LinkInfo
                {
                    Page = pagination.CurrentPage - 1,
                    Text = "Previous",
                    IsActive = false,
                    IsDisabled = !pagination.HasPreviousPage,
                    PageKey = pagination.PageKey,
                    Fragments = pagination.Fragments
                };
                case LinkInfoType.Next: return new LinkInfo
                {
                    Page = pagination.CurrentPage + 1,
                    Text = "Next",
                    IsActive = false,
                    IsDisabled = !pagination.HasNextPage,
                    PageKey = pagination.PageKey,
                    Fragments = pagination.Fragments
                };
                case LinkInfoType.Regular: return new LinkInfo
                {
                    Page = page,
                    Text = page.ToString(),
                    IsActive = false,
                    IsDisabled = false,
                    PageKey = pagination.PageKey,
                    Fragments = pagination.Fragments
                };
                case LinkInfoType.Current: return new LinkInfo
                {
                    Page = page,
                    Text = page.ToString(),
                    IsActive = true,
                    IsDisabled = false,
                    PageKey = pagination.PageKey,
                    Fragments = pagination.Fragments
                };
                default: throw new System.InvalidOperationException("Wrong type");
            }
        }
    }
}
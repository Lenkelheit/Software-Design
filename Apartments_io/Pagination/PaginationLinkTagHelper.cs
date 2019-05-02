using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pagination
{
    public class PaginationLinkTagHelper : TagHelper
    {
        // FIELDS
        private IUrlHelperFactory urlHelperFactory;

        // CONSTRUCTORS
        public PaginationLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        // PROPERTIES
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public Pagination PaginationModel { get; set; }

        // METHODS
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            output.Content.SetHtmlContent(PaginationModel.Build(GetUrlInfo()));
        }
        private DataTransferObject.UrlInfo GetUrlInfo()
        {
            return new DataTransferObject.UrlInfo()
            {
                UrlHelper = urlHelperFactory.GetUrlHelper(ViewContext),
                ActionName = ActionName,
                ControllerName = ControllerName
            };
        }
    }
}

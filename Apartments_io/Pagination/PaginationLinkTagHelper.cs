using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pagination
{
    /// <summary>
    /// Tag helper for pagination
    /// </summary>
    public class PaginationLinkTagHelper : TagHelper
    {
        // FIELDS
        private IUrlHelperFactory urlHelperFactory;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="PaginationLinkTagHelper"/>
        /// </summary>
        /// <param name="helperFactory">
        /// A factory for creating <see cref="Microsoft.AspNetCore.Mvc.IUrlHelper"/>
        /// </param>
        public PaginationLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        // PROPERTIES
        /// <summary>
        /// Context in which current tag helper has been used
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets or sets controller name
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// Gets or sets action name
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Gets or sets pagination model
        /// </summary>
        public Pagination PaginationModel { get; set; }

        // METHODS
        /// <summary>
        /// Executes TagHelper with the given context and output
        /// </summary>
        /// <param name="context">
        /// Contains information associated with the current HTML tag
        /// </param>
        /// <param name="output">
        /// A stateful HTML element used to generate an HTML tag
        /// </param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // if only one page..
            if (PaginationModel.TotalPagesAmount <= 1)
            {
                // .. do not show pagination at all
                output.SuppressOutput();
            }
            else
            {
                // .. build pagination
                output.TagName = "div";

                output.Content.SetHtmlContent(PaginationModel.Build(GetUrlInfo()));
            }
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

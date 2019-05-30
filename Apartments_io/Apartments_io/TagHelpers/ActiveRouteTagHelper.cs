using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Apartments_io.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        // FIELDS
        private IDictionary<string, string> routeValues;

        // PROPERTIES
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (this.routeValues == null)
                {
                    this.routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }
                return this.routeValues;
            }
            set
            {
                this.routeValues = value;
            }
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        // METHODS
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (IsActive())
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool IsActive()
        {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString();
            string currentAction = ViewContext.RouteData.Values["Action"].ToString();

            if (!string.IsNullOrWhiteSpace(Controller) && Controller.ToLower() != currentController.ToLower())
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Action) && Action.ToLower() != currentAction.ToLower())
            {
                return false;
            }

            foreach (KeyValuePair<string, string> routeValue in RouteValues)
            {
                if (!ViewContext.RouteData.Values.ContainsKey(routeValue.Key) ||
                    ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            output.AddClass("active", System.Text.Encodings.Web.HtmlEncoder.Default);
        }
    }
}

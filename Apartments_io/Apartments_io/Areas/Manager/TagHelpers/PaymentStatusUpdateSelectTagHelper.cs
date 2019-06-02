using System;

using DataAccess.Enums;

using Apartments_io.Areas.Manager.Models;

using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System.Text.Encodings.Web;

namespace Apartments_io.Areas.Manager.TagHelpers
{
    public class PaymentStatusUpdateSelectTagHelper : TagHelper
    {
        public PaymentStatus SelectedStatus { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";

            // disabled select if is not new bill
            if (SelectedStatus != PaymentStatus.WaitingForPayment)
            {
                output.Attributes.Add("disabled", "disabled");
            }

            // add classes for select
            output.AddClass(SelectedStatus.ToCssClass(), HtmlEncoder.Default);

            // add option
            foreach (PaymentStatus status in Enum.GetValues(typeof(PaymentStatus)))
            {
                bool isDisabled = IsOptionDisabledForManager(status);
                bool isSelected = SelectedStatus == status;
                
                output.Content.AppendHtml($"<option value='{Convert.ToInt32(status)}' {DisabledOption(isDisabled)} {SelectedOption(isSelected)}>{status.GetText()}</option>");
            }
        }
        private bool IsOptionDisabledForManager(PaymentStatus status)
        {
            // is new bill
            // manager can choose between: WaitingForPayment or Overdue
            return SelectedStatus == PaymentStatus.WaitingForPayment && (status == PaymentStatus.Paid || status == PaymentStatus.PaidWithDelay);
        }
        private string DisabledOption(bool isDisaled)
        {
            return isDisaled ? "disabled" : String.Empty;
        }

        private string SelectedOption(bool isSelected)
        {
            return isSelected ? "selected" : String.Empty;
        }
    }
}

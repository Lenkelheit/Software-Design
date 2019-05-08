namespace Apartments_io.Areas.Manager.Models
{
    public static class PaymentStatusText
    {
        public static string GetText(this DataAccess.Enums.PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case DataAccess.Enums.PaymentStatus.WaitingForPayment: return "Waiting for payment";
                case DataAccess.Enums.PaymentStatus.Paid: return "Paid";
                case DataAccess.Enums.PaymentStatus.Overdue: return "Overdue";
                case DataAccess.Enums.PaymentStatus.PaidWithDelay: return "Paid with delay";

                default: throw new System.InvalidOperationException("Wrong enum value");
            }
        }
    }
}

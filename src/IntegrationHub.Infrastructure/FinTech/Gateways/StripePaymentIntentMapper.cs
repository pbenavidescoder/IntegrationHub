using IntegrationHub.Domain.FinTech.Enums;

namespace IntegrationHub.Infrastructure.FinTech.Gateways
{
    public static class StripePaymentIntentMapper
    {
        public static PaymentStatus MapStatus(string stripeStatus)
        {
            return stripeStatus switch
            {
                "succeeded" => PaymentStatus.Completed,
                "canceled" => PaymentStatus.Cancelled,
                "requires_payment_method" => PaymentStatus.Failed,
                "processing" => PaymentStatus.Pending,
                _ => PaymentStatus.Failed
            };
        }
    }
}

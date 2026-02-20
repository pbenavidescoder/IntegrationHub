using IntegrationHub.Domain.FinTech.Entities;
using IntegrationHub.Domain.FinTech.Enums;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using IntegrationHub.Domain.FinTech.ValueObjects;
using IntegrationHub.Infrastructure.FinTech.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace IntegrationHub.Infrastructure.FinTech.Gateways
{
    public class StripePaymentGateway : IPaymentGateway
    {
        private readonly StripeSettings _stripeSettings;

        public StripePaymentGateway(IOptions<StripeSettings> options)
        {
            _stripeSettings = options.Value;
            StripeConfiguration.ApiKey = _stripeSettings.ApiKey;
        }
        public PaymentGatewayResult Charge(Payment payment)
        {
            try
            {
                var unitAmount = Convert.ToInt32(payment.Amount * 100);
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card", "amazon_pay" },
                    Mode = "payment",
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = payment.Currency,
                            UnitAmount = unitAmount,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = payment.ProductDescription,
                                Description = $"Merchant: {payment.Merchant}"
                            }
                        },
                        Quantity =1
                    }
                },
                    SuccessUrl = _stripeSettings.SuccessUrl + "?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = _stripeSettings.CancelUrl,
                    Metadata = new Dictionary<string, string>
                    {
                        {"accountId", payment.AccountId.ToString() }
                    }
                    
                };

                var service = new SessionService();
                var session = service.Create(options);

                return new PaymentGatewayResult(
                    paymentId: payment.PaymentId,
                    success: true,
                    externalTransactionId: session.Id,
                    errorMessage: string.Empty,
                    status: PaymentStatus.Pending,
                    checkoutUrl: session.Url
                    );
            }
            catch (StripeException ex)
            {
                return new PaymentGatewayResult(
                    paymentId: payment.PaymentId,
                    success: false,
                    externalTransactionId: string.Empty,
                    errorMessage: $"Stripe error: {ex.Message}",
                    status: PaymentStatus.Failed,
                    checkoutUrl: string.Empty
                    );
            }
            catch (HttpRequestException ex)
            {
                return new PaymentGatewayResult(
                    paymentId: payment.PaymentId,
                    success: false,
                    externalTransactionId: string.Empty,
                    errorMessage: $"Network error: {ex.Message}",
                    status: PaymentStatus.Failed,
                    checkoutUrl: string.Empty
                    );
            }
            catch (Exception ex)
            {
                return new PaymentGatewayResult(
                    paymentId: payment.PaymentId,
                    success: false,
                    externalTransactionId: string.Empty,
                    errorMessage: $"Unexpected error: {ex.Message}",
                    status: PaymentStatus.Failed,
                    checkoutUrl: string.Empty
                    );
            }
        }
    }
}

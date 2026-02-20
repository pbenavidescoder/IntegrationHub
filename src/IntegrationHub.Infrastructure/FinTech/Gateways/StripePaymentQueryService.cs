using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Application.FinTech.OutboundPorts;
using IntegrationHub.Infrastructure.FinTech.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.V2.Core;

namespace IntegrationHub.Infrastructure.FinTech.Gateways
{
    public class StripePaymentQueryService : IPaymentQueryService
    {
        private readonly StripeSettings _stripeSettings;

        public StripePaymentQueryService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.ApiKey;

        }
        public async Task<PaymentDetails> GetPaymentBySessionIdAsync(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            return new PaymentDetails
            {
                SessionId = session.Id,
                Amount = session.AmountTotal / 100.0m,
                Currency = session.Currency,
                Status = session.PaymentStatus,
                PaymentIntent = session.PaymentIntentId
            };
        }
    }
}

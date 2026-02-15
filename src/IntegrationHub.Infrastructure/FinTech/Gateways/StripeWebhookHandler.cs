using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Application.FinTech.InboundPorts;
using IntegrationHub.Domain.FinTech.Enums;
using IntegrationHub.Infrastructure.FinTech.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace IntegrationHub.Infrastructure.FinTech.Gateways
{
    public class StripeWebhookHandler : IPaymentWebHookHandler
    {
        private readonly StripeSettings _settings;

        public StripeWebhookHandler(IOptions<StripeSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<WebhookResult?> HandleAsync(string payload, string signature)
        {
            var stripeEvent = EventUtility.ConstructEvent(payload, signature, _settings.WebhookKey);

            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null)
                    throw new InvalidDataException("Stripe session missing"); // TODO: add Stripe exeptions
                
                var intentService = new PaymentIntentService();
                var intent = await intentService.GetAsync(session.PaymentIntentId);

                var status = StripePaymentIntentMapper.MapStatus(intent.Status);

                return new WebhookResult(session.Metadata["accountId"], session.PaymentIntentId, status);
            }
            
            return null;
        }
    }
}

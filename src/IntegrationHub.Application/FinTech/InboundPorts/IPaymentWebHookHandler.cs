

using IntegrationHub.Application.FinTech.DTOs;

namespace IntegrationHub.Application.FinTech.InboundPorts
{
    public interface IPaymentWebHookHandler
    {
        Task<WebhookResult?> HandleAsync(string payload, string signature);
    }
}

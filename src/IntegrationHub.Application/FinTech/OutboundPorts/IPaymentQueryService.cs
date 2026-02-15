using IntegrationHub.Application.FinTech.DTOs;

namespace IntegrationHub.Application.FinTech.OutboundPorts
{
   public interface IPaymentQueryService
    {
        public Task<PaymentDetails> GetPaymentBySessionIdAsync(string sessionId);
    }
}

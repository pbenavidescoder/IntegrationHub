using IntegrationHub.Domain.FinTech.Enums;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record WebhookResult(string AccountId, string PaymentId, PaymentStatus Status)
    {
    }
}

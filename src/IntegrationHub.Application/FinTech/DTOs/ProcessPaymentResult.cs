using IntegrationHub.Domain.FinTech.Enums;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record ProcessPaymentResult(Guid accountId, Guid paymentId, string? externalTransactionId, bool success, PaymentStatus status, decimal amount, string currency, string? message, string? checkOutUr);
}

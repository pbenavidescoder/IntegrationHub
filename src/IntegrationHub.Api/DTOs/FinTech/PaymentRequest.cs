namespace IntegrationHub.Api.DTOs.FinTech
{
    public class PaymentRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public string Currency { get; set; } = "$";
        public required string PaymentMethod { get; set; }
        public string? MerchantId { get; set; }

    }
}

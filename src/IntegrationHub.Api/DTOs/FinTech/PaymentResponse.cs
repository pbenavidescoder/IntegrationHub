namespace IntegrationHub.Api.DTOs.FinTech
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string? ExternalTransactionId { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? CheckOutUrl { get; set; }

    }
}

namespace IntegrationHub.Api.DTOs.FinTech
{
    public class CreateAccountRequest
    {
        public required string OwnerName { get; set; }
        public string? Email { get; set; }
        public string Currency { get; set; } = "$";
        public decimal InitialAmount { get; set; }

    }
}

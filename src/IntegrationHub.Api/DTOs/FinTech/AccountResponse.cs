namespace IntegrationHub.Api.DTOs.FinTech
{
    public class AccountResponse
    {
        public Guid AccountId { get; set; }
        public string OwnerName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = default!;
    }
}

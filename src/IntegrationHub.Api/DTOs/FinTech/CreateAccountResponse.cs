namespace IntegrationHub.Api.DTOs.FinTech
{
    public class CreateAccountResponse
    {
        public Guid AccountId { get; set; }
        public string OwnerName { get; set; } = default!;
        public decimal Balance { get; set; }
    }
}

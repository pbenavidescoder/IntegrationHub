namespace IntegrationHub.Api.DTOs.FinTech
{
    public class WithdrawFundsRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}

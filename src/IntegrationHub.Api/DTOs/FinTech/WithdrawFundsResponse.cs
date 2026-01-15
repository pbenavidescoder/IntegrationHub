namespace IntegrationHub.Api.DTOs.FinTech
{
    public class WithdrawFundsResponse
    {
        public Guid AccountId { get; set; }
        public decimal NewBalance { get; set; }
    }
}

namespace IntegrationHub.Api.DTOs.FinTech
{
    public class DepositFundsResponse
    {
        public Guid AccountId{ get; set; }
        public decimal NewBalance { get; set; }
    }
}

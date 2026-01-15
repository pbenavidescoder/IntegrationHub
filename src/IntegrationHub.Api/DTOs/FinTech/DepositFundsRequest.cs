namespace IntegrationHub.Api.DTOs.FinTech
{
    public class DepositFundsRequest
    {
        public Guid AccountId { get; set; }
        public decimal Amount{ get; set; }
    }
}

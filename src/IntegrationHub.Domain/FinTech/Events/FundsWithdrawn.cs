using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Events
{
    public class FundsWithdrawn : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;

        public string EventType { get; } = nameof(FundsWithdrawn);
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public string? Reason { get; }

        public FundsWithdrawn(Guid accountId, decimal amount, string? reason = null)
        {
            AccountId = accountId;
            Amount = amount;
            Reason = reason;
        }
    }
}

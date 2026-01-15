using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Events
{
    public class PaymentFailed : IDomainEvent
    {
        public Guid EventId {get;} = Guid.NewGuid();

        public DateTime OccurredOn { get;} = DateTime.UtcNow;

        public string EventType { get; } = nameof(PaymentFailed);
        public Guid PaymentId { get; }
        public Guid AccountId { get; }
        public decimal Amount { get; }
        public string? Reason { get; }

        public PaymentFailed(Guid paymentId, Guid accountId, decimal amount, string? reason = null)
        {
            PaymentId = paymentId;
            AccountId = accountId;
            Amount = amount;
            Reason = reason;

        }
    }
}

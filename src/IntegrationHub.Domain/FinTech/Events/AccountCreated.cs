using IntegrationHub.Domain.FinTech.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Events
{
    public class AccountCreated : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string EventType { get; } = nameof(AccountCreated);
        public Guid AccountId { get; set; }
        public string Owner { get; }
        public string Email { get; }

        public AccountCreated(Guid account, string owner, string email)
        {
            AccountId = account;
            Owner = owner;
            Email = email;
        }
        
    }
}

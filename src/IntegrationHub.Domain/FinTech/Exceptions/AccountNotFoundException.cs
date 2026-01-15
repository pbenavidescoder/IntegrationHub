using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Exceptions
{
    public class AccountNotFoundException : DomainException
    {
        public AccountNotFoundException(Guid accountId) 
            : base($"Account {accountId} not found", "ACCOUNT_NOT_FOUND") { }
    }
}

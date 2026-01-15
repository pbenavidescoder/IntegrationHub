using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Exceptions
{
    public class InsufficientFundsException : DomainException
    {    
       public InsufficientFundsException(Guid accountId, decimal amount)
            : base($"Account {accountId} has inssuficient funds for withdrawal of {amount}", "INSUFFICIENT_FUNDS") { }
        
    }
}

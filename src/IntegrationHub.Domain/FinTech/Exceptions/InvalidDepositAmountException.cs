using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Exceptions
{
    public class InvalidDepositAmountException : DomainException
    {
        public InvalidDepositAmountException(Guid accountId, decimal amount)
            : base($"Deposit of {amount} is not allowed for account {accountId}.", "INVALID_DEPOSIT_AMOUNT") { }
    }
}

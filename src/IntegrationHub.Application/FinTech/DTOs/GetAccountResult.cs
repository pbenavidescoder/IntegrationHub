using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record GetAccountResult(Guid AccountId, string Owner, string EmailAddress, decimal Balance, string Currency);
  
}

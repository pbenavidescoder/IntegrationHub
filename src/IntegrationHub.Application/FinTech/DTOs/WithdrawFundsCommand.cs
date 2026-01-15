using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record WithdrawFundsCommand(Guid AccountId, decimal Amount, string Reason);
}

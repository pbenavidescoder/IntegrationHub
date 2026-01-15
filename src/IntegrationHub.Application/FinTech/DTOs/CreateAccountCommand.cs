using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record CreateAccountCommand(string AccountOwner, string Email, string Currency, decimal Balance);
    
}

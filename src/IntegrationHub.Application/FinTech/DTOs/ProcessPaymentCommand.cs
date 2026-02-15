using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public record ProcessPaymentCommand(Guid accountID, decimal amount, string description, string currency, string method, string merchantId);
}

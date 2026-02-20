using IntegrationHub.Domain.FinTech.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.DTOs
{
    public class PaymentDetails
    {
        public required string SessionId { get; init; }
        public decimal? Amount { get; init; }
        public required string Currency { get; init; }
        public required string Status { get; init; }
        public required string PaymentIntent { get; set; }

    }
}

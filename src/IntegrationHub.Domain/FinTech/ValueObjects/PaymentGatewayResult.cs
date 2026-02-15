using IntegrationHub.Domain.FinTech.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.ValueObjects
{
    public class PaymentGatewayResult
    {
        public Guid PaymentId { get; }
        public bool Success { get; }
        public string? ExternalTransactionId { get; }
        public string ErrorMessage { get; } 
        public PaymentStatus Status{ get; }

        public string? CheckOutUrl { get; }

        public PaymentGatewayResult(Guid paymentId, bool success,string externalTransactionId, string errorMessage, PaymentStatus status, string checkoutUrl)
        {
            PaymentId = paymentId;
            Success = success;
            ExternalTransactionId = externalTransactionId;
            ErrorMessage = errorMessage;
            Status = status;
            CheckOutUrl = checkoutUrl;
        }
    }
}

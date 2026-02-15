using IntegrationHub.Domain.FinTech.Entities;
using IntegrationHub.Domain.FinTech.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.OutboundPorts
{
    public interface IPaymentGateway
    {
        public PaymentGatewayResult Charge(Payment payment);
    }
}

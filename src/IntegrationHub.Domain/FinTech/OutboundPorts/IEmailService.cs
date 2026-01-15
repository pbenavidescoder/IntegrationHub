using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.OutboundPorts
{
    public interface IEmailService
    {
        Task SendAsync(string to, string body);
    }
}

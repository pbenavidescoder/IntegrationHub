using IntegrationHub.Domain.FinTech.OutboundPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Infrastructure.FinTech.Email
{
    public class ConsoleEmailService : IEmailService
    {
        public Task SendAsync(string to, string body)
        {
            Console.WriteLine($"Simulatind email sent to {to}: {body}");
            return Task.CompletedTask;
        }
    }
}

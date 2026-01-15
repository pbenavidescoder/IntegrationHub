using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.Handlers
{
    public class SendWelcomeEmailHandler : IEventHandler<AccountCreated>
    {
        private readonly IEmailService _emailService;

        public SendWelcomeEmailHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public Task Handle(AccountCreated evt)
        {            
            var msj = $"[Handler:SendWelcomeEmail] - Account Created for {evt.Owner} - ({evt.Email})";
            return _emailService.SendAsync(evt.Email, msj);
        }
    }
}

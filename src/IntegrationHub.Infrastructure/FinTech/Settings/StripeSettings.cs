using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Infrastructure.FinTech.Settings
{
    public class StripeSettings
    {
        public required string ApiKey { get; set; }
        public required string SuccessUrl { get; set; }
        public required string CancelUrl { get; set; }
        public required string WebhookKey { get; set; }
    }
}

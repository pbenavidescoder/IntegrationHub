using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Events
{
   public interface IEventBus
    {
        void Publish(IDomainEvent domainEvent);
    }
}

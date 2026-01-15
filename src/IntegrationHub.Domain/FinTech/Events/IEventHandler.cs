using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Events
{
    public interface IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        Task Handle(TEvent evt);
    }
}

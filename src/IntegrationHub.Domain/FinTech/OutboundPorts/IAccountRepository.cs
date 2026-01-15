using IntegrationHub.Domain.FinTech.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.OutboundPorts
{
    public interface IAccountRepository
    {
        Account GetById(Guid accountId);
        IEnumerable<Account> GetAll();
        void Save(Account account);

    }
}

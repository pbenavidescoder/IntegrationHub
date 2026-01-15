using IntegrationHub.Domain.FinTech.Entities;
using IntegrationHub.Domain.FinTech.Exceptions;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Infrastructure.FinTech.Persistence
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly Dictionary<Guid, Account> _accounts = new();
        public Account GetById(Guid accountId)
        {
            if (_accounts.TryGetValue(accountId, out var account))
                return account;
            throw new AccountNotFoundException(accountId);
        }
        public IEnumerable<Account> GetAll() => _accounts.Values;
      

        public void Save(Account account)
        {
            _accounts[account.AccountId]= account;
        }
    }
}

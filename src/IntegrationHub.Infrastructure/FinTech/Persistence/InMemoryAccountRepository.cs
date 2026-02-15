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
        public Task<Account> GetByIdAsync(Guid accountId)
        {
            if (_accounts.TryGetValue(accountId, out var account))
                return Task.FromResult(account);
            throw new AccountNotFoundException(accountId);
        }
        public IEnumerable<Account> GetAll() => _accounts.Values;
      

        public Task SaveAsync(Account account)
        {
            _accounts[account.AccountId]= account;
            return Task.CompletedTask;
        }
    }
}

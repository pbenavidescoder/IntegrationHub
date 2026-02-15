using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Domain.FinTech.Entities;
using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.OutboundPorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Application.FinTech.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEventBus _eventBus;

        public AccountService(IAccountRepository accountRepository, IEventBus eventBus)
        {
            _accountRepository = accountRepository;
            _eventBus = eventBus;
        }

        public IEnumerable<GetAccountResult> ListAccounts()
        {
            var accounts =  _accountRepository.GetAll();
            return accounts.Select(a => new GetAccountResult(
                a.AccountId,
                a.OwnerName,
                a.Email.Address,
                a.Balance,
                a.Currency));
        }

        public async Task<CreateAccountResult> CreateAccount(CreateAccountCommand command)
        {
            var account = new Account(command.AccountOwner, command.Email, command.Currency, command.Balance);
            await _accountRepository.SaveAsync(account);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();

            return new CreateAccountResult(account.AccountId, account.OwnerName, account.Balance);
        }

        public async Task<DepositFundsResult> DepositFunds(DepositFundsCommand command)
        {
            var account = await _accountRepository.GetByIdAsync(command.AccountId);
            account.Deposit(command.Amount);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();

            await _accountRepository.SaveAsync(account);

            return new DepositFundsResult(account.AccountId, account.Balance);
        }

        public async Task<WithdrawFundsResult> WithdrawFunds(WithdrawFundsCommand command)
        {
            var account = await _accountRepository.GetByIdAsync(command.AccountId);
            account.Withdraw(command.Amount, command.Reason);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();
            await _accountRepository.SaveAsync(account);

            return new WithdrawFundsResult(account.AccountId, account.Balance);
        }
    }
}

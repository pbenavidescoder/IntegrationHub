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
            var accounts =_accountRepository.GetAll();
            return accounts.Select(a => new GetAccountResult(
                a.AccountId,
                a.OwnerName,
                a.Email.Address,
                a.Balance,
                a.Currency));
        }

        public CreateAccountResult CreateAccount(CreateAccountCommand command)
        {
            var account = new Account(command.AccountOwner, command.Email, command.Currency, command.Balance);
            _accountRepository.Save(account);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();

            return new CreateAccountResult(account.AccountId, account.OwnerName, account.Balance);
        }

        public DepositFundsResult DepositFunds(DepositFundsCommand command)
        {
            var account = _accountRepository.GetById(command.AccountId);
            account.Deposit(command.Amount);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();

            _accountRepository.Save(account);

            return new DepositFundsResult(account.AccountId, account.Balance);
        }

        public WithdrawFundsResult WithdrawFunds(WithdrawFundsCommand command)
        {
            var account = _accountRepository.GetById(command.AccountId);
            account.Withdraw(command.Amount, command.Reason);

            foreach (var domainEvent in account.DomainEvents)
            {
                _eventBus.Publish(domainEvent);
            }

            account.ClearDomainEvents();
            _accountRepository.Save(account);

            return new WithdrawFundsResult(account.AccountId, account.Balance);
        }
    }
}

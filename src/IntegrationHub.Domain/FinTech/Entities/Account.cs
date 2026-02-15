using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.Exceptions;
using IntegrationHub.Domain.FinTech.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Entities
{
    /// <summary>
    /// Represents a financial account within the FinTech domain.
    /// </summary>
    /// <remarks>
    /// This entity models an account with a unique identifier, owner information, 
    /// email address, balance, and currency. It provides methods to deposit and 
    /// withdraw funds while enforcing domain rules such as positive amounts and 
    /// sufficient balance for withdrawals.
    /// </remarks>
    public class Account
    {
        /// <summary>
        /// Represents a collection of payments associated with the current instance.
        /// </summary>
        /// <remarks>This field is a read-only list that stores payment information. It is initialized as
        /// an empty list and cannot be reassigned after construction. Use this field to manage or access payment data
        /// within the class.</remarks>
        private readonly List<Payment> _payments = new();

        /// <summary>
        /// Gets the unique identifier of the account.
        /// </summary>
        public Guid AccountId { get; private set; }

        /// <summary>
        /// Gets the name of the account owner.
        /// </summary>
        public string OwnerName { get; private set; }

        /// <summary>
        /// Gets the email address associated with the account.
        /// </summary>
        /// <remarks>
        /// Currently stored as a string. Consider replacing with the <see cref="ValueObjects.Email"/> 
        /// value object for stronger validation and immutability.
        /// </remarks>
        public Email Email { get; private set; }

        /// <summary>
        /// Gets the current balance of the account.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Gets the currency in which the account balance is maintained.
        /// </summary>
        public string Currency { get; private set; }

        /// <summary>
        /// Gets the collection of payments associated with the current instance.
        /// </summary>
        public IReadOnlyCollection<Payment> Payments
        {
            get { return _payments.AsReadOnly(); }
        }

        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents
        {
            get { return _domainEvents.AsReadOnly(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="ownerName">The name of the account owner.</param>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="currency">The currency of the account balance.</param>
        /// <remarks>
        /// The account is initialized with a new <see cref="AccountId"/>, a balance of 0, 
        /// and the specified currency.
        /// </remarks>
        public Account(string ownerName, string emailAddress, string currency, decimal balance =0m)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
                throw new ArgumentException("[Account] - Owner is required");

            AccountId = Guid.NewGuid();
            OwnerName = ownerName;
            Email = new Email(emailAddress);
            Balance = balance;
            Currency = currency;

            _domainEvents.Add(new AccountCreated(AccountId, OwnerName, Email.Address));
        }
        
        /// <summary>
        /// Deposits the specified amount into the account.
        /// </summary>
        /// <param name="amount">The amount to deposit.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the deposit amount is less than or equal to zero.
        /// </exception>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidDepositAmountException(AccountId, amount);
            Balance += amount;
            _domainEvents.Add(new FundsDeposited(AccountId, amount));
        }

        /// <summary>
        /// Withdraws the specified amount from the account.
        /// </summary>
        /// <param name="amount">The amount to withdraw.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the withdrawal amount is less than or equal to zero.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the withdrawal amount exceeds the current balance.
        /// </exception>
        public void Withdraw(decimal amount, string? reason = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");

            if (amount > Balance)
                throw new InsufficientFundsException(AccountId, amount);
            Balance -= amount;
            _domainEvents.Add(new FundsWithdrawn(AccountId, amount, reason));
        }

        public Payment MakePayment(decimal amount, string reason, string currency, string method, string merchant)
        {
            
            if (amount <= 0) throw new ArgumentException("Amount must be greater than zero"); //TODO add specific exception.
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required");
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentException("Payment method is required");

            var payment = Payment.CreatePayment(AccountId, amount, currency, method, reason, merchant);
            _payments.Add(payment);

            _domainEvents.Add(new PaymentCreated(payment.PaymentId, payment.AccountId, payment.Amount));

            return payment;
        }

        public void AttachPaymentExternalId(string paymentId, string externalId)
        {
            var payment = _payments.FirstOrDefault(p => p.PaymentId.ToString() == paymentId);
            if (payment == null) throw new InvalidOperationException("Payment not found");

            payment.AttachExternalId(externalId);

        }

        public void CompletePayment(string externalId)
        {
            var payment = _payments.FirstOrDefault(p => p.ExternalId == externalId);
            if (payment == null)
                throw new InvalidOperationException("Payment not found");
            
            Withdraw(payment.Amount, payment.ProductDescription);
            payment.Complete();

            _domainEvents.Add(new PaymentCompleted(payment.PaymentId, payment.AccountId, payment.Amount));

        }

        public void FailPayment(Payment payment, string reason)
        {
            payment.Fail(reason);
            _domainEvents.Add(new PaymentFailed(payment.PaymentId, payment.AccountId, payment.Amount, payment.FailureReason));
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}

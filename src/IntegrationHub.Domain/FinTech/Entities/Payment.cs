using IntegrationHub.Domain.FinTech.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace IntegrationHub.Domain.FinTech.Entities
{
    /// <summary>
    /// Represents a payment transaction within the FinTech domain.
    /// </summary>
    /// <remarks>
    /// This entity models the lifecycle of a payment, including its unique identifier, 
    /// associated account, amount, date, status, and optional metadata such as merchant 
    /// and payment method. It enforces domain rules to ensure that only pending payments 
    /// can be completed or failed.
    /// </remarks>
    public class Payment
    {
        /// <summary>
        /// Gets the unique identifier of the payment.
        /// </summary>
        public Guid PaymentId { get; private set; }

        /// <summary>
        /// Gets the identifier of the account associated with this payment.
        /// </summary>
        public Guid AccountId { get; private set; }

        /// <summary>
        /// Gets the monetary amount of the payment.
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Gets the date and time when the payment was created.
        /// </summary>
        public DateTime PaymentDate { get; private set; }

        /// <summary>
        /// Gets the current status of the payment.
        /// </summary>
        public PaymentStatus Status { get; private set; }

        /// <summary>
        /// Gets the reason for failure if the payment has failed.
        /// </summary>
        public string FailureReason { get; private set; }

        /// <summary>
        /// Gets the merchant associated with the payment, if applicable.
        /// </summary>
        public string? Merchant { get; private set; }

        /// <summary>
        /// Gets the payment method used for the transaction, if applicable.
        /// </summary>
        public string? PaymentMethod { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Payment"/> class.
        /// </summary>
        /// <param name="accountGuid">The identifier of the account associated with the payment.</param>
        /// <param name="amount">The monetary amount of the payment.</param>
        /// <remarks>
        /// The payment is initialized with a new <see cref="PaymentId"/>, the current UTC date/time, 
        /// a status of <see cref="PaymentStatus.Pending"/>, and an empty failure reason.
        /// </remarks>
        private Payment(Guid accountGuid, decimal amount)
        {
            PaymentId = Guid.NewGuid();
            AccountId = accountGuid;
            Amount = amount;
            PaymentDate = DateTime.UtcNow;
            Status = PaymentStatus.Pending;
            FailureReason = string.Empty;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Payment"/> class with the specified account identifier and amount.
        /// </summary>
        /// <param name="accountGuid">The unique identifier of the account associated with the payment.</param>
        /// <param name="amount">The payment amount. Must be a positive value.</param>
        /// <returns>A new <see cref="Payment"/> instance initialized with the specified account identifier and amount.</returns>
        internal static Payment Create(Guid accountGuid, decimal amount)
        {
            return new Payment(accountGuid, amount);
        }

        /// <summary>
        /// Marks the payment as completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the payment is not in a <see cref="PaymentStatus.Pending"/> state.
        /// </exception>
        internal void Complete()
        {
            if (Status != PaymentStatus.Pending)
            {
                throw new InvalidOperationException("Only pending payments can be completed.");
            }
            Status = PaymentStatus.Completed;
        }

        /// <summary>
        /// Marks the payment as failed and records the failure reason.
        /// </summary>
        /// <param name="reason">The reason why the payment failed.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the payment is not in a <see cref="PaymentStatus.Pending"/> state.
        /// </exception>
        internal void Fail(string reason)
        {
            if (Status != PaymentStatus.Pending)
            {
                throw new InvalidOperationException("Only pending payments can be failed.");
            }
            Status = PaymentStatus.Failed;
            FailureReason = reason;
        }
    }
}

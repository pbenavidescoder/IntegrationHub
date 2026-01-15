using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Enums
{
    /// <summary>
    /// Represents the possible states of a payment transaction 
    /// within the FinTech domain.
    /// </summary>
    /// <remarks>
    /// This enumeration is used to track and manage the lifecycle 
    /// of a payment. Each value indicates a distinct status that 
    /// can be applied to a Payment transaction.
    /// </remarks>
    public enum PaymentStatus
    {
        /// <summary>
        /// The payment has been initiated but not yet processed.
        /// </summary>
        Pending,

        /// <summary>
        /// The payment was successfully processed and completed.
        /// </summary>
        Completed,
        
        /// <summary>
        /// The payment attempt failed due to an error or rejection.
        /// </summary>
        Failed,

        // <summary>
        /// The payment was successfully refunded to the payer.
        /// </summary>
        Refunded,

        /// <summary>
        /// The payment was cancelled before completion.
        /// </summary>
        Cancelled
    }
}

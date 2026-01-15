using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.ValueObjects
{
    /// <summary>
    /// Represents an inmuitable email address value object withing the FinTech domain.
    /// </summary>
    /// <remarks>
    /// This class enforces basic validation to ensure the email address is not null, empty, or whitespace,
    /// and contains an "@" character. It overrides equality and hash codes members to allow comparison based on the email address value.
    /// and provides an implicit conversion to string for ease of use.
    public class Email
    {
        /// <summary>
        /// Gets the validated email address.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Email"/> class with the specified email address.
        /// </summary>
        /// <param name="address">The email address to be encapsulated. </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided email address is null, empty, whitespace, or does not contain an "@" character.
        /// </exception>
        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address) || (!address.Contains("@")))
                throw new ArgumentException("Invalid email address");
            
            Address = address;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Email"/> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if the specified object is an <see cref="Email"/> and its <see cref="Address"/>
        /// matches the current instance's <see cref="Address"/> using a case-insensitive comparison; otherwise, <see
        /// langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Email email && Address.Equals(email.Address, StringComparison.OrdinalIgnoreCase); 
        }

        /// <summary>
        /// Returns a hash code for the current <see cref="Email"/> instance.
        /// </summary>
        /// <returns>
        /// A hash code based on the email address value, using case-insensitive comparison.
        /// </returns>
        public override int GetHashCode() => Address.GetHashCode(StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Provides an implicit conversion from <see cref="Email"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="email">The <see cref="Email"/> instance to convert.</param>
        /// <returns>The underlying email address string.</returns>

        public static implicit operator string(Email email) => email.Address;

        /// <summary>
        /// Returns the string representation of the email address.
        /// </summary>
        /// <returns>The email address as a string.</returns>
        public override string ToString() => Address;

       
    }
}

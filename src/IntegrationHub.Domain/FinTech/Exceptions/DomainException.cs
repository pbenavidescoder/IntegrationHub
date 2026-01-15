using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationHub.Domain.FinTech.Exceptions
{
    public abstract class DomainException : Exception
    {
        public string ErrorCode { get; }

        protected DomainException(string message, string errorCode): base(message)
        {
            ErrorCode = errorCode;
        }
    }
}

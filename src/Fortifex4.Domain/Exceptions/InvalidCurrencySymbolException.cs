using System;

namespace Fortifex4.Domain.Exceptions
{
    public class InvalidCurrencySymbolException : Exception
    {
        public InvalidCurrencySymbolException()
        {
        }

        public InvalidCurrencySymbolException(string message) : base(message)
        {
        }

        public InvalidCurrencySymbolException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
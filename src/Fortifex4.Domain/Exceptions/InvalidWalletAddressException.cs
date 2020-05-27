using System;

namespace Fortifex4.Domain.Exceptions
{
    public class InvalidWalletAddressException : Exception
    {
        public InvalidWalletAddressException()
        {
        }

        public InvalidWalletAddressException(string message) : base(message)
        {
        }

        public InvalidWalletAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidWalletAddressException(string walletAddress, string blockchainSymbol) : base($"{blockchainSymbol} Wallet Address [{walletAddress}] is invalid.")
        {
        }
    }
}
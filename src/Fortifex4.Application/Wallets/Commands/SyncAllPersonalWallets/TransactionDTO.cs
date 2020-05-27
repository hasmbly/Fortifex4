using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Wallets.Commands.SyncAllPersonalWallets
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public string TransactionHash { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
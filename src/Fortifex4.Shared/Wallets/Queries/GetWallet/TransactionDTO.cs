using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Wallets.Queries.GetWallet
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public int PocketID { get; set; }
        public string TransactionHash { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }
        public string TransactionTypeDisplayText { get; set; }

        public int? TradeID { get; set; }
        public int? InternalTransferID { get; set; }
    }
}
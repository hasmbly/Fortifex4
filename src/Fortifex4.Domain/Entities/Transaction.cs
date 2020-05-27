using System;
using System.Collections.Generic;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        public int TransactionID { get; set; }
        public int PocketID { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string TransactionHash { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }

        public Pocket Pocket { get; set; }

        public IList<InternalTransfer> FromInternalTransfers { get; set; }
        public IList<InternalTransfer> ToInternalTransfers { get; set; }
        public IList<Trade> FromTrades { get; set; }
        public IList<Trade> ToTrades { get; set; }

        public Transaction()
        {
            this.FromInternalTransfers = new List<InternalTransfer>();
            this.ToInternalTransfers = new List<InternalTransfer>();
            this.FromTrades = new List<Trade>();
            this.ToTrades = new List<Trade>();
        }

        public string TransactionTypeDisplayText
        {
            get
            {
                return this.TransactionType switch
                {
                    TransactionType.StartingBalance => "Starting Balance",
                    TransactionType.Deposit => "Deposit",
                    TransactionType.Withdrawal => "Withdrawal",
                    TransactionType.ExternalTransferIN => "External Transfer (IN)",
                    TransactionType.ExternalTransferOUT => "External Transfer (OUT)",
                    TransactionType.InternalTransferIN => "Internal Transfer (IN)",
                    TransactionType.InternalTransferOUT => "Internal Transfer (OUT)",
                    TransactionType.BuyIN => "Buy (IN)",
                    TransactionType.BuyOUT => "Buy (OUT)",
                    TransactionType.SellIN => "Sell (IN)",
                    TransactionType.SellOUT => "Sell (OUT)",
                    TransactionType.SyncTransactionIN => "Sync (IN)",
                    TransactionType.SyncTransactionOUT => "Sync (OUT)",
                    TransactionType.SyncBalanceImport => "Balance Import",
                    _ => "Unknown",
                };
            }
        }
    }
}
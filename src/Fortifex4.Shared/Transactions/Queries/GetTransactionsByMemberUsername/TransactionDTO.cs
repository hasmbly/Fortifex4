using Fortifex4.Domain.Enums;
using System;

namespace Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public string SourceProviderName { get; set; }
        public string DestinationProviderName { get; set; }
        public string SourceWalletName { get; set; }
        public string DestinationWalletName { get; set; }
        public string SourceCurrencySymbol { get; set; }
        public string DestinationCurrencySymbol { get; set; }
        public string SourceCurrencyName { get; set; }
        public string DestinationCurrencyName { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public decimal UnitPrice{ get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public TransactionType TransactionType { get; set; }
        public string TransactionTypeDisplayText { get; set; }

        public int? TradeID { get; set; }
        public int? InternalTransferID { get; set; }

        public string AmountDisplayText
        {
            get
            {
                return this.Amount.ToString("N4").Replace(".0000", "");
            }
        }

        public string UnitPriceInUSDDisplayText
        {
            get
            {
                return this.UnitPriceInUSD.ToString("N4").Replace(".0000", "");
            }

        }

        public string UnitPriceDisplayText
        {
            get
            {
                return this.UnitPrice.ToString("N4").Replace(".0000", "");
            }

        }

        public decimal TotalPrice
        {
            get
            {
                return this.Amount * this.UnitPrice;
            }
        }

        public string TotalPriceDisplayText
        {
            get
            {
                return this.TotalPrice.ToString("N4").Replace(".0000", "");
            }
        }
    }
}
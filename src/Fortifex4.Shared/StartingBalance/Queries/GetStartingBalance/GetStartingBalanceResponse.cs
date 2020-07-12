using System;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.StartingBalance.Queries.GetStartingBalance
{
    public class GetStartingBalanceResponse : GeneralResponse
    {
        public string WalletName { get; set; }
        public string WalletOwnerProviderName { get; set; }
        public string WalletMainPocketCurrencyName { get; set; }
        public string WalletMainPocketCurrencySymbol { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }

        public string WalletNameWithProviderName 
        { 
            get
            {
                return $"{this.WalletOwnerProviderName} - {this.WalletName}";
            }
            set
            {
                return;
            }
        }
    }
}
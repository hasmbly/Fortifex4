using System;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer
{
    public class GetExternalTransferResponse : GeneralResponse
    {
        public string WalletName { get; set; }
        public string WalletOwnerProviderName { get; set; }
        public string WalletMainPocketCurrencyName { get; set; }
        public string WalletMainPocketCurrencySymbol { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }

        public decimal AbsoluteAmount
        {
            get
            {
                return Math.Abs(this.Amount);
            }
        }
    }
}
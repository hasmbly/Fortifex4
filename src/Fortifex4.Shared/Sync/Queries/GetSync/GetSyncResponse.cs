using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Sync.Queries.GetSync
{
    public class GetSyncResponse
    {
        public string WalletName { get; set; }
        public string WalletMainPocketCurrencyName { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }
    }
}
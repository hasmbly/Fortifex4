using Fortifex4.Domain.Enums;
using System;

namespace Fortifex4.Shared.Trades.Queries.GetTrade
{
    public class GetTradeResponse
    {
        public int TradeID { get; set; }
        public int OwnerID { get; set; }
        public int SourceCurrencyID { get; set; }
        public int DestinationCurrencyID { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public TradeType TradeType { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public bool IsWithholding { get; set; }

        public decimal AbsoluteAmount
        {
            get
            {
                return Math.Abs(this.Amount);
            }
        }
    }
}
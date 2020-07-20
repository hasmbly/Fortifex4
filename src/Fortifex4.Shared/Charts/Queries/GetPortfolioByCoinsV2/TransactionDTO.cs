using System;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPriceInUSD { get; set; }
    }
}
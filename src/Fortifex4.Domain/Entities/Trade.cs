using Fortifex4.Domain.Enums;

namespace Fortifex4.Domain.Entities
{
    public class Trade
    {
        public int TradeID { get; set; }
        public int FromTransactionID { get; set; }
        public int ToTransactionID { get; set; }
        public TradeType TradeType { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsWithholding { get; set; }

        public Transaction FromTransaction { get; set; }
        public Transaction ToTransaction { get; set; }
    }
}
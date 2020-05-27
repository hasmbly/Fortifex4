using System;

namespace Fortifex4.Application.InternalTransfers.Queries.GetInternalTransfer
{
    public class GetInternalTransferResult
    {
        public int InternalTransferID { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public int FromTransactionPocketID { get; set; }
        public int FromTransactionPocketWalletID { get; set; }
        public string FromWalletName { get; set; }
        public string ToWalletName { get; set; }
        public string CurrencyName { get; set; }
    }
}
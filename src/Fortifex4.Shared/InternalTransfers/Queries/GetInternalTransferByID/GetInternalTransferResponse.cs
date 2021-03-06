﻿using System;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer
{
    public class GetInternalTransferResponse : GeneralResponse
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
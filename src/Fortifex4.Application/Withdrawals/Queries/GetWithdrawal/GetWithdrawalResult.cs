using System;

namespace Fortifex4.Application.Withdrawals.Queries.GetWithdrawal
{
    public class GetWithdrawalResult
    {
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }

        public decimal AbsoluteAmount
        {
            get
            {
                return Math.Abs(this.Amount);
            }
        }
    }
}
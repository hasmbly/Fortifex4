using System;

namespace Fortifex4.Shared.Withdrawals.Queries.GetWithdrawal
{
    public class GetWithdrawalResponse
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
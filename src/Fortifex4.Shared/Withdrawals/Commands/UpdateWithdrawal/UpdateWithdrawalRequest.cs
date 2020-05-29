using MediatR;
using System;

namespace Fortifex4.Shared.Withdrawals.Commands.UpdateWithdrawal
{
    public class UpdateWithdrawalRequest : IRequest<UpdateWithdrawalResponse>
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }

    }
}
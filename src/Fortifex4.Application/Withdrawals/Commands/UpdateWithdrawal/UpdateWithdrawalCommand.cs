using MediatR;
using System;

namespace Fortifex4.Application.Withdrawals.Commands.UpdateWithdrawal
{
    public class UpdateWithdrawalCommand : IRequest<UpdateWithdrawalResult>
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }

    }
}
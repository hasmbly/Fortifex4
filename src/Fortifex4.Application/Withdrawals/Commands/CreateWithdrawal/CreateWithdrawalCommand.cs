using MediatR;
using System;

namespace Fortifex4.Application.Withdrawals.Commands.CreateWithdrawal
{
    public class CreateWithdrawalCommand : IRequest<CreateWithdrawalResult>
    {
        public int WalletID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
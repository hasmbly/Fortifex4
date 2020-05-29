using MediatR;
using System;

namespace Fortifex4.Shared.Withdrawals.Commands.CreateWithdrawal
{
    public class CreateWithdrawalRequest : IRequest<CreateWithdrawalResponse>
    {
        public int WalletID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
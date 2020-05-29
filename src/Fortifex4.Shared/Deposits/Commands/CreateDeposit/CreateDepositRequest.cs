using System;
using MediatR;

namespace Fortifex4.Shared.Deposits.Commands.CreateDeposit
{
    public class CreateDepositRequest : IRequest<CreateDepositResponse>
    {
        public int? WalletID { get; set; }
        public int OwnerID { get; set; }
        public int CurrencyID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
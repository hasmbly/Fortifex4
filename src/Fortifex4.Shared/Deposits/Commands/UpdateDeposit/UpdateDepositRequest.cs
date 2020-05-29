using MediatR;
using System;

namespace Fortifex4.Shared.Deposits.Commands.UpdateDeposit
{
    public class UpdateDepositRequest : IRequest<UpdateDepositResponse>
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }   
    }
}
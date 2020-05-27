using MediatR;
using System;

namespace Fortifex4.Application.Deposits.Commands.UpdateDeposit
{
    public class UpdateDepositCommand : IRequest<UpdateDepositResult>
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDateTime { get; set; }   
    }
}
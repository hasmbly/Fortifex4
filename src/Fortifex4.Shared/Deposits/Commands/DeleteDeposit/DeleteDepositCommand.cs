using MediatR;

namespace Fortifex4.Application.Deposits.Commands.DeleteDeposit
{
    public class DeleteDepositCommand : IRequest<DeleteDepositResult>
    {
        public int TransactionID { get; set; }
    }
}
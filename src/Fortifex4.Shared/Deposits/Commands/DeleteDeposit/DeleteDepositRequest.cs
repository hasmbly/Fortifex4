using MediatR;

namespace Fortifex4.Shared.Deposits.Commands.DeleteDeposit
{
    public class DeleteDepositRequest : IRequest<DeleteDepositResponse>
    {
        public int TransactionID { get; set; }
    }
}
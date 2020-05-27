using MediatR;

namespace Fortifex4.Application.Withdrawals.Commands.DeleteWithdrawal
{
    public class DeleteWithdrawalCommand : IRequest<DeleteWithdrawalResult>
    {
        public int TransactionID { get; set; }
    }
}

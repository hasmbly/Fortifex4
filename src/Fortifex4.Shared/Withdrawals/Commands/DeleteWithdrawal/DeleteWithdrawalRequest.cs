using MediatR;

namespace Fortifex4.Shared.Withdrawals.Commands.DeleteWithdrawal
{
    public class DeleteWithdrawalRequest : IRequest<DeleteWithdrawalResponse>
    {
        public int TransactionID { get; set; }
    }
}
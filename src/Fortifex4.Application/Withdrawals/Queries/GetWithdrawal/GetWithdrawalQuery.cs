using MediatR;

namespace Fortifex4.Application.Withdrawals.Queries.GetWithdrawal
{
    public class GetWithdrawalQuery : IRequest<GetWithdrawalResult>
    {
        public int TransactionID { get; set; }
    }
}
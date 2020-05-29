using MediatR;

namespace Fortifex4.Shared.Withdrawals.Queries.GetWithdrawal
{
    public class GetWithdrawalRequest : IRequest<GetWithdrawalResponse>
    {
        public int TransactionID { get; set; }
    }
}
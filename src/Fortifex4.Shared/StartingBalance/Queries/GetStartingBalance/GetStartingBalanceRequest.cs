using MediatR;

namespace Fortifex4.Shared.StartingBalance.Queries.GetStartingBalance
{
    public class GetStartingBalanceRequest : IRequest<GetStartingBalanceResponse>
    {
        public int TransactionID { get; set; }
    }
}
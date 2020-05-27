using MediatR;

namespace Fortifex4.Shared.Charts.Queries.GetCoinByExchanges
{
    public class GetCoinByExchangesRequest : IRequest<GetCoinByExchangesResponse>
    {
        public string MemberUsername { get; set; }
        public int CurrencyID { get; set; } = 0;
    }
}
using MediatR;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByExchanges
{
    public class GetPortfolioByExchangesRequest : IRequest<GetPortfolioByExchangesResponse>
    {
        public string MemberUsername { get; set; }
    }
}
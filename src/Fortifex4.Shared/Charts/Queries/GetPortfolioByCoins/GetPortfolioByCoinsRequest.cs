using MediatR;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoins
{
    public class GetPortfolioByCoinsRequest : IRequest<GetPortfolioByCoinsResponse>
    {
        public string MemberUsername { get; set; }
    }
}
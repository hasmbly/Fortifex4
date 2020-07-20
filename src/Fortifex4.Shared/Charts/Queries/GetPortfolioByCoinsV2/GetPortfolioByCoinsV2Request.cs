using MediatR;

namespace Fortifex4.Shared.Charts.Queries.GetPortfolioByCoinsV2
{
    public class GetPortfolioByCoinsV2Request : IRequest<GetPortfolioByCoinsV2Response>
    {
        public string MemberUsername { get; set; }
    }
}
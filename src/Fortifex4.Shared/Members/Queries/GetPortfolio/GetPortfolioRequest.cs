using MediatR;

namespace Fortifex4.Shared.Members.Queries.GetPortfolio
{
    public class GetPortfolioRequest : IRequest<GetPortfolioResponse>
    {
        public string MemberUsername { get; set; }
    }
}
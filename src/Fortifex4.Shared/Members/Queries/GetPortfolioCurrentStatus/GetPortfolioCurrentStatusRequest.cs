using MediatR;

namespace Fortifex4.Shared.Members.Queries.GetPortfolioCurrentStatus
{
    public class GetPortfolioCurrentStatusRequest : IRequest<GetPortfolioCurrentStatusResponse>
    {
        public string MemberUsername { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Application.Members.Queries.GetPortfolioCurrentStatus
{
    public class GetPortfolioCurrentStatusQuery : IRequest<GetPortfolioCurrentStatusResult>
    {
        public string MemberUsername { get; set; }
    }
}
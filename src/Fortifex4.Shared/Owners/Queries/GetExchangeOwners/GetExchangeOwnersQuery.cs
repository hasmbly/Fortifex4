using MediatR;

namespace Fortifex4.Shared.Owners.Queries.GetExchangeOwners
{
    public class GetExchangeOwnersRequest : IRequest<GetExchangeOwnersResponse>
    {
        public string MemberUsername { get; set; }
    }
}
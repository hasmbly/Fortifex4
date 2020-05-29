using MediatR;

namespace Fortifex4.Shared.Lookup.Queries.GetOwners
{
    public class GetOwnersRequest : IRequest<GetOwnersResponse>
    {
        public string MemberUsername { get; set; }
    }
}
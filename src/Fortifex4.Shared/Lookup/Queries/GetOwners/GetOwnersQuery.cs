using MediatR;

namespace Fortifex4.Application.Lookup.Queries.GetOwners
{
    public class GetOwnersQuery : IRequest<GetOwnersResult>
    {
        public string MemberUsername { get; set; }
    }
}
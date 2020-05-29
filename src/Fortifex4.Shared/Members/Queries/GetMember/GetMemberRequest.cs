using MediatR;

namespace Fortifex4.Shared.Members.Queries.GetMember
{
    public class GetMemberRequest : IRequest<GetMemberResponse>
    {
        public string MemberUsername { get; set; }
    }
}
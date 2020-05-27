using MediatR;

namespace Fortifex4.Application.Members.Queries.GetMember
{
    public class GetMemberQuery : IRequest<GetMemberResult>
    {
        public string MemberUsername { get; set; }
    }
}
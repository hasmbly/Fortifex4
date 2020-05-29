using MediatR;

namespace Fortifex4.Shared.Members.Commands.CreateMember
{
    public class CreateMemberRequest : IRequest<CreateMemberResponse>
    {
        public string MemberUsername { get; set; }
        public string Password { get; set; }
    }
}
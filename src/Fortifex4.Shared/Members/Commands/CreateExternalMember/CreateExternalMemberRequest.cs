using MediatR;

namespace Fortifex4.Shared.Members.Commands.CreateExternalMember
{
    public class CreateExternalMemberRequest : IRequest<CreateExternalMemberResponse>
    {
        public string ExternalID { get; set; }
        public string ClaimType { get; set; }
        public string FullName { get; set; }
        public string MemberUsername { get; set; }
        public string PictureURL { get; set; }
    }
}
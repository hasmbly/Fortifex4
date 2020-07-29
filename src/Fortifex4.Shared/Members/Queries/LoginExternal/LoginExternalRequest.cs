using MediatR;

namespace Fortifex4.Shared.Members.Queries.LoginExternal
{
    public class LoginExternalRequest : IRequest<LoginExternalResponse>
    {
        public string MemberUsername { get; set; }
        public string ExternalID { get; set; }
        public string AuthenticationScheme { get; set; }
        public string FullName { get; set; }
        public string PictureURL { get; set; }
    }
}
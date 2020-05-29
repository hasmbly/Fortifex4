using MediatR;

namespace Fortifex4.Shared.Members.Queries.Login
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string MemberUsername { get; set; }
        public string Password { get; set; }
    }
}
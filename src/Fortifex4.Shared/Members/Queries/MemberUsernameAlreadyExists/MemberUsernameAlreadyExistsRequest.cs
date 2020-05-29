using MediatR;

namespace Fortifex4.Shared.Members.Queries.MemberUsernameAlreadyExists
{
    public class MemberUsernameAlreadyExistsRequest : IRequest<MemberUsernameAlreadyExistsResponse>
    {
        public string MemberUsername { get; set; }
    }
}
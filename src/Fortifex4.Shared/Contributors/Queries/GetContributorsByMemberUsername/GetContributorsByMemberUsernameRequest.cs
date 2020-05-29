using MediatR;

namespace Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameRequest : IRequest<GetContributorsByMemberUsernameResponse>
    {
        public string MemberUsername { get; set; }
    }
}
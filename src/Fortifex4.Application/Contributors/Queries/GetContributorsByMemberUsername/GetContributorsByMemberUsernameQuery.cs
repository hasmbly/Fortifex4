using MediatR;

namespace Fortifex4.Application.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameQuery : IRequest<GetContributorsByMemberUsernameResult>
    {
        public string MemberUsername { get; set; }
    }
}
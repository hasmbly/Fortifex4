using MediatR;

namespace Fortifex4.Shared.Contributors.Queries.CheckIsContributor
{
    public class CheckIsContributorRequest : IRequest<CheckIsContributorResponse>
    {
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
    }
}
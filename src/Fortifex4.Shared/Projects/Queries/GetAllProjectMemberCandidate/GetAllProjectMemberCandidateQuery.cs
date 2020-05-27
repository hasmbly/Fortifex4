using MediatR;

namespace Fortifex4.Application.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateQuery : IRequest<GetAllProjectMemberCandidateResult>
    {
        public string ExcludeCreatorUsername { get; set; }
    }
}
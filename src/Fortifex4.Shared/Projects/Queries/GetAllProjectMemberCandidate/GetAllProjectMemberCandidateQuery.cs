using MediatR;

namespace Fortifex4.Shared.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateRequest: IRequest<GetAllProjectMemberCandidateResponse>
    {
        public string ExcludeCreatorUsername { get; set; }
    }
}
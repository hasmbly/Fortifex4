using System.Collections.Generic;

namespace Fortifex4.Application.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateResult
    {
        public IList<MembersDTO> Members { get; set; }

        public GetAllProjectMemberCandidateResult()
        {
            this.Members = new List<MembersDTO>();
        }
    }
}
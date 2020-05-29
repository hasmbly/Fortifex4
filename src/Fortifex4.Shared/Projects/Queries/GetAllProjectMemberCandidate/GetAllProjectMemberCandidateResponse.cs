using System.Collections.Generic;

namespace Fortifex4.Shared.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateResponse
    {
        public IList<MembersDTO> Members { get; set; }

        public GetAllProjectMemberCandidateResponse()
        {
            this.Members = new List<MembersDTO>();
        }
    }
}
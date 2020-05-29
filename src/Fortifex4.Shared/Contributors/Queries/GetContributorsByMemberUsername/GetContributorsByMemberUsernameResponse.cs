using System.Collections.Generic;

namespace Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameResponse
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public GetContributorsByMemberUsernameResponse()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
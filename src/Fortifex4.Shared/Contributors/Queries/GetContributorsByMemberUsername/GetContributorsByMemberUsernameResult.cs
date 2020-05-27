using System.Collections.Generic;

namespace Fortifex4.Application.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameResult
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public GetContributorsByMemberUsernameResult()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
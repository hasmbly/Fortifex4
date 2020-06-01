using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameResponse : GeneralResponse
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public GetContributorsByMemberUsernameResponse()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
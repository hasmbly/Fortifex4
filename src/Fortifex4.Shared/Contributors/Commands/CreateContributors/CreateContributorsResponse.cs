using System.Collections.Generic;

namespace Fortifex4.Shared.Contributors.Commands.CreateContributors
{
    public class CreateContributorsResponse
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public CreateContributorsResponse()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
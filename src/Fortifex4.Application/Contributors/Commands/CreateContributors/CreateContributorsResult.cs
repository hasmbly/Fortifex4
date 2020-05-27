using System.Collections.Generic;

namespace Fortifex4.Application.Contributors.Commands.CreateContributors
{
    public class CreateContributorsResult
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public CreateContributorsResult()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Contributors.Commands.CreateContributors
{
    public class CreateContributorsResponse : GeneralResponse
    {
        public IList<ContributorDTO> Contributors { get; set; }

        public CreateContributorsResponse()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
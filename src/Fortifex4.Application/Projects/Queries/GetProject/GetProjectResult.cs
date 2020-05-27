using Fortifex4.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Fortifex4.Application.Projects.Queries.GetProject
{
    public class GetProjectResult
    {
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string BlockchainName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WalletAddress { get; set; }
        public bool IsExistProjectByMemberUsernameResult { get; set; }

        public IList<ContributorDTO> Contributors { get; set; }

        public GetProjectResult()
        {
            this.Contributors = new List<ContributorDTO>();
        }
    }
}
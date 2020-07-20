using System.Collections.Generic;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Projects.Queries.GetProject
{
    public class GetProjectResponse : GeneralResponse
    {
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string BlockchainName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WalletAddress { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public bool IsExistProjectByMemberUsernameResult { get; set; }

        public IList<ContributorDTO> Contributors { get; set; }
        public IList<ProjectDocumentDTO> ProjectDocuments { get; set; }

        public GetProjectResponse()
        {
            this.Contributors = new List<ContributorDTO>();
            this.ProjectDocuments = new List<ProjectDocumentDTO>();
        }
    }
}
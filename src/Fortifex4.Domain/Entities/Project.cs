using System.Collections.Generic;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WalletAddress { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        public Member Member { get; set; }
        public Blockchain Blockchain { get; set; }

        public IList<ProjectStatusLog> ProjectStatusLogs { get; private set; }
        public IList<ProjectDocument> ProjectDocuments { get; private set; }
        public IList<Contributor> Contributors { get; private set; }

        public Project()
        {
            this.ProjectStatusLogs = new List<ProjectStatusLog>();
            this.ProjectDocuments = new List<ProjectDocument>();
            this.Contributors = new List<Contributor>();
        }
    }
}
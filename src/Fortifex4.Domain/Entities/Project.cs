using System.Collections.Generic;
using Fortifex4.Domain.Common;

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

        public Member Member { get; set; }
        public Blockchain Blockchain { get; set; }

        public IList<Contributor> Contributors { get; private set; }

        public Project()
        {
            this.Contributors = new List<Contributor>();
        }
    }
}
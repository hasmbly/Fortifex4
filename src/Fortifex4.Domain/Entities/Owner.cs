using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Owner : AuditableEntity
    {
        public int OwnerID { get; set; }
        public string MemberUsername { get; set; }
        public int ProviderID { get; set; }
        public ProviderType ProviderType { get; set; }

        public Member Member { get; set; }
        public Provider Provider { get; set; }

        public IList<Wallet> Wallets { get; private set; }

        public Owner()
        {
            this.Wallets = new List<Wallet>();
        }
    }
}
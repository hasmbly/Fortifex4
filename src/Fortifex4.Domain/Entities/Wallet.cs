using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Wallet: AuditableEntity
    {
        public int WalletID { get; set; }
        public int OwnerID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsSynchronized { get; set; }
        public ProviderType ProviderType { get; set; }

        public Owner Owner { get; set; }
        public Blockchain Blockchain { get; set; }

        public IList<Pocket> Pockets { get; set; }

        public Wallet()
        {
            this.Pockets = new List<Pocket>();
        }
    }
}
using System.Collections.Generic;
using Fortifex4.Shared.Owners.Common;

namespace Fortifex4.Shared.Owners.Queries.GetOwner
{
    public class GetOwnerResponse : WalletContainer
    {
        public int OwnerID { get; set; }
        public string MemberUsername { get; set; }
        public int ProviderID { get; set; }

        public string ProviderName { get; set; }
        public string ProviderSiteURL { get; set; }

        public IList<WalletDTO> Wallets { get; set; }

        public GetOwnerResponse()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
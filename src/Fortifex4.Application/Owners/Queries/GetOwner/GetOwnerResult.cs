using Fortifex4.Application.Owners.Common;
using System.Collections.Generic;

namespace Fortifex4.Application.Owners.Queries.GetOwner
{
    public class GetOwnerResult : WalletContainer
    {
        public int OwnerID { get; set; }
        public string MemberUsername { get; set; }
        public int ProviderID { get; set; }

        public string ProviderName { get; set; }
        public string ProviderSiteURL { get; set; }

        public IList<WalletDTO> Wallets { get; set; }

        public GetOwnerResult()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
using Fortifex4.Shared.Owners.Common;
using System.Collections.Generic;

namespace Fortifex4.Shared.Owners.Queries.GetExchangeOwners
{
    public class ExchangeOwnerDTO
    {
        public int OwnerID { get; set; }
        public int ProviderID { get; set; }

        public string ProviderName { get; set; }
        public string ProviderSiteURL { get; set; }

        public IList<WalletDTO> ExchangeWallets { get; set; }

        public ExchangeOwnerDTO()
        {
            this.ExchangeWallets = new List<WalletDTO>();
        }
    }
}
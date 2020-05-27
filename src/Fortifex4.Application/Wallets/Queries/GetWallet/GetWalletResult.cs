using Fortifex4.Domain.Enums;
using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Queries.GetWallet
{
    public class GetWalletResult
    {
        public int WalletID { get; set; }
        public int OwnerID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ProviderType ProviderType { get; set; }
        public bool IsSynchronized { get; set; }

        public string BlockchainName { get; set; }
        public string OwnerProviderName { get; set; }
        public PocketDTO MainPocket { get; set; }

        public IList<PocketDTO> TokenPockets { get; set; }

        public GetWalletResult()
        {
            this.TokenPockets = new List<PocketDTO>();
        }
    }
}
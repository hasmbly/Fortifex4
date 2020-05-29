using System.Collections.Generic;

namespace Fortifex4.Shared.Wallets.Commands.SyncAllPersonalWallets
{
    public class SyncAllPersonalWalletsResponse
    {
        public IList<WalletDTO> Wallets { get; set; }

        public SyncAllPersonalWalletsResponse()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
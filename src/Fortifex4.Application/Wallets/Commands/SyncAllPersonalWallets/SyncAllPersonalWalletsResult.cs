using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Commands.SyncAllPersonalWallets
{
    public class SyncAllPersonalWalletsResult
    {
        public IList<WalletDTO> Wallets { get; set; }

        public SyncAllPersonalWalletsResult()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
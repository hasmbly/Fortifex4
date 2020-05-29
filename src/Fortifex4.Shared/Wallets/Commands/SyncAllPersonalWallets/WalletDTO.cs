using System.Collections.Generic;

namespace Fortifex4.Shared.Wallets.Commands.SyncAllPersonalWallets
{
    public class WalletDTO
    {
        public int WalletID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string BlockchainSymbol { get; set; }

        public IList<PocketDTO> Pockets { get; set; }

        public WalletDTO()
        {
            this.Pockets = new List<PocketDTO>();
        }
    }
}
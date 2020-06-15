using System.Collections.Generic;

namespace Fortifex4.Shared.Wallets.Common
{
    public class WalletDTO
    {
        public int WalletID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }

        public string BlockchainSymbol { get; set; }
        public string BlockchainName { get; set; }

        public IList<PocketDTO> Pockets { get; set; }

        public WalletDTO()
        {
            this.Pockets = new List<PocketDTO>();
        }
    }
}
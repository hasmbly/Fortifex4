using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class GetWalletsBySameUsernameAndBlockchainResult
    {
        public IList<WalletDTO> Wallets { get; set; }

        public GetWalletsBySameUsernameAndBlockchainResult()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
using System.Collections.Generic;

namespace Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain
{
    public class GetAllWalletsBySameUsernameAndBlockchainResponse
    {
        public List<AllWalletDTO> Wallets { get; set; }

        public GetAllWalletsBySameUsernameAndBlockchainResponse()
        {
            this.Wallets = new List<AllWalletDTO>();
        }
    }
}
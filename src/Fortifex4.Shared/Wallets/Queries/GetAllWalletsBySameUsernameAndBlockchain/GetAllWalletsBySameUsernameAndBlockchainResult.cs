using System.Collections.Generic;

namespace Fortifex4.Application.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain
{
    public class GetAllWalletsBySameUsernameAndBlockchainResult
    {
        public List<AllWalletDTO> Wallets { get; set; }

        public GetAllWalletsBySameUsernameAndBlockchainResult()
        {
            this.Wallets = new List<AllWalletDTO>();
        }
    }
}
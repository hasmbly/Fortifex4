using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class GetWalletsBySameUsernameAndBlockchainResponse : GeneralResponse
    {
        public IList<WalletDTO> Wallets { get; set; }

        public GetWalletsBySameUsernameAndBlockchainResponse()
        {
            this.Wallets = new List<WalletDTO>();
        }
    }
}
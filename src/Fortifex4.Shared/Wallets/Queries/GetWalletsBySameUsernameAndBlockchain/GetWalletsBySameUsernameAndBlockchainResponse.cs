using System.Collections.Generic;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Wallets.Common;

namespace Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class GetWalletsBySameUsernameAndBlockchainResponse : GeneralResponse
    {
        public IList<WalletSameCurrencyDTO> Wallets { get; set; }

        public GetWalletsBySameUsernameAndBlockchainResponse()
        {
            this.Wallets = new List<WalletSameCurrencyDTO>();
        }
    }
}
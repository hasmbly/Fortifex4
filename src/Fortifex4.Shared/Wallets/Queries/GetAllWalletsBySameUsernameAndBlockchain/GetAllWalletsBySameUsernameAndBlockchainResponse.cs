using System.Collections.Generic;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Wallets.Common;

namespace Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain
{
    public class GetAllWalletsBySameUsernameAndBlockchainResponse : GeneralResponse
    {
        public IList<WalletSameCurrencyDTO> Wallets { get; set; }

        public GetAllWalletsBySameUsernameAndBlockchainResponse()
        {
            this.Wallets = new List<WalletSameCurrencyDTO>();
        }
    }
}
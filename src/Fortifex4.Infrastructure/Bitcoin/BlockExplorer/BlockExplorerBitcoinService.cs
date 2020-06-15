using System;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Infrastructure.Constants;
using Fortifex4.Infrastructure.Common;
using Fortifex4.Application.Common.Interfaces;

namespace Fortifex4.Infrastructure.Bitcoin.BlockExplorer
{
    public class BlockExplorerBitcoinService : IBitcoinService
    {
        public async Task<CryptoWallet> GetBitcoinWalletAsync(string address)
        {
            var result = new CryptoWallet();

            //https://blockexplorer.com/api/addr/19SokJG7fgk8iTjemJ2obfMj14FM16nqzj
            string uri = $"{BitcoinServiceProviders.BlockExplorer.AddrEndpointURL}/{address}";

            var walletJSON = await ExternalWebAPIRequestor.GetAsync<WalletJSON>(uri);

            result.Balance = walletJSON.balance;

            return result;
        }

        public Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}
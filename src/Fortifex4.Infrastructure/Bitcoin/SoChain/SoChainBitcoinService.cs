using System;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Bitcoin;

namespace Fortifex4.Infrastructure.Bitcoin.SoChain
{
    public class SoChainBitcoinService : IBitcoinService
    {
        public Task<BitcoinWallet> GetBitcoinWalletAsync(string address)
        {
            throw new NotImplementedException();
        }

        public Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}
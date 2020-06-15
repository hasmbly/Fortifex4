using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;

namespace Fortifex4.Infrastructure.Bitocin.Fake
{
    public class FakeBitcoinService : IBitcoinService
    {
        public async Task<CryptoWallet> GetBitcoinWalletAsync(string address)
        {
            return await Task.FromResult(new CryptoWallet { Balance = 777m });
        }

        public async Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            return await Task.FromResult(new BitcoinTransactionCollection());
        }
    }
}
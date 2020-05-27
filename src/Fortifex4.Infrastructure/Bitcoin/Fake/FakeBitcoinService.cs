using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Bitcoin;

namespace Fortifex4.Infrastructure.Bitocin.Fake
{
    public class FakeBitcoinService : IBitcoinService
    {
        public async Task<BitcoinWallet> GetBitcoinWalletAsync(string address)
        {
            return await Task.FromResult(new BitcoinWallet { Balance = 777m });
        }

        public async Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address)
        {
            return await Task.FromResult(new BitcoinTransactionCollection());
        }
    }
}
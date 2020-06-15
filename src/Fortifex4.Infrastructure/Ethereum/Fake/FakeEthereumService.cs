using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Ethereum;

namespace Fortifex4.Infrastructure.Ethereum.Fake
{
    public class FakeEthereumService : IEthereumService
    {
        public async Task<CryptoWallet> GetEthereumWalletAsync(string address)
        {
            return await Task.FromResult(new CryptoWallet { Balance = 777m });
        }

        public async Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address)
        {
            return await Task.FromResult(new EthereumTransactionCollection());
        }
    }
}
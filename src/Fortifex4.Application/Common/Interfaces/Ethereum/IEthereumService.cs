using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Ethereum
{
    public interface IEthereumService
    {
        Task<CryptoWallet> GetEthereumWalletAsync(string address);
        Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address);
    }
}
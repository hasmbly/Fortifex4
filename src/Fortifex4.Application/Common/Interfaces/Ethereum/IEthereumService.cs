using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Ethereum
{
    public interface IEthereumService
    {
        Task<EthereumWallet> GetEthereumWalletAsync(string address);
        Task<EthereumTransactionCollection> GetEthereumTransactionCollectionAsync(string address);
    }
}
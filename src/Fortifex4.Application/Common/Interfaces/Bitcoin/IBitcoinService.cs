using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Bitcoin
{
    public interface IBitcoinService
    {
        Task<CryptoWallet> GetBitcoinWalletAsync(string address);
        Task<BitcoinTransactionCollection> GetBitcoinTransactionCollectionAsync(string address);
    }
}
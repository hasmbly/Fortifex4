using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Dogecoin
{
    public interface IDogecoinService
    {
        Task<CryptoWallet> GetDogecoinWalletAsync(string address);
        Task<DogecoinTransactionCollection> GetDogecoinTransactionCollectionAsync(string address);
    }
}
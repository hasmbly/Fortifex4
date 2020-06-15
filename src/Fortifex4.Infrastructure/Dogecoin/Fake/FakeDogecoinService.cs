using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Dogecoin;

namespace Fortifex4.Infrastructure.Dogecoin.Fake
{
    public class FakeDogecoinService : IDogecoinService
    {
        public async Task<CryptoWallet> GetDogecoinWalletAsync(string address)
        {
            return await Task.FromResult(new CryptoWallet { Balance = 777m });
        }

        public async Task<DogecoinTransactionCollection> GetDogecoinTransactionCollectionAsync(string address)
        {
            return await Task.FromResult(new DogecoinTransactionCollection());
        }
    }
}
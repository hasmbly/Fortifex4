using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Hive;

namespace Fortifex4.Infrastructure.Hive.Fake
{
    public class FakeHiveService : IHiveService
    {
        public async Task<CryptoWallet> GetHiveWalletAsync(string address)
        {
            return await Task.FromResult(new CryptoWallet { Balance = 777m });
        }
    }
}
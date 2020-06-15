using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Steem;

namespace Fortifex4.Infrastructure.Steem.Fake
{
    public class FakeSteemService : ISteemService
    {
        public async Task<CryptoWallet> GetSteemWalletAsync(string address)
        {
            return await Task.FromResult(new CryptoWallet { Balance = 777m });
        }
    }
}
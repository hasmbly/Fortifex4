using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Steem
{
    public interface ISteemService
    {
        Task<CryptoWallet> GetSteemWalletAsync(string address);
    }
}
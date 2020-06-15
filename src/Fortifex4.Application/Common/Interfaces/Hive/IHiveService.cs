using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Hive
{
    public interface IHiveService
    {
        Task<CryptoWallet> GetHiveWalletAsync(string address);
    }
}
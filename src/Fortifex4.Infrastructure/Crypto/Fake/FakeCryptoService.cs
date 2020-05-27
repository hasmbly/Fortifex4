using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Crypto;

namespace Fortifex4.Infrastructure.Crypto.Fake
{
    public class FakeCryptoService : ICryptoService
    {
        public async Task<CryptoBlockchainCollection> GetCryptoBlockchainCollectionAsync()
        {
            return await Task.FromResult(new CryptoBlockchainCollection());
        }

        public async Task<CryptoLatestQuotesResult> GetLatestQuoteAsync(string fromCurrencySymbol, string toCurrencySymbol)
        {
            return await Task.FromResult(new CryptoLatestQuotesResult());
        }

        public async Task<decimal> ConvertAsync(string fromCurrencySymbol, string toCurrencySymbol, decimal amount)
        {
            return await Task.FromResult(777m);
        }

        public async Task<decimal> GetUnitPriceAsync(string fromCurrencySymbol, string toCurrencySymbol)
        {
            return await Task.FromResult(777m);
        }
    }
}
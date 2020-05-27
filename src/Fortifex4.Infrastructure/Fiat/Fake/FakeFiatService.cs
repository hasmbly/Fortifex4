using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces.Fiat;

namespace Fortifex4.Infrastructure.Fiat.Fake
{
    public class FakeFiatService : IFiatService
    {
        public async Task<FiatCurrencyCollection> GetFiatCurrencyCollectionAsync()
        {
            return await Task.FromResult(new FiatCurrencyCollection());
        }

        public async Task<FiatExchangeRateCollection> GetFiatExchangeRateCollectionAsync(string baseCurrencySymbol)
        {
            return await Task.FromResult(new FiatExchangeRateCollection());
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
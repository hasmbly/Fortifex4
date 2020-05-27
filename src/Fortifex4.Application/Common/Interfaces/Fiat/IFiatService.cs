using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Fiat
{
    public interface IFiatService
    {
        Task<FiatCurrencyCollection> GetFiatCurrencyCollectionAsync();
        Task<FiatExchangeRateCollection> GetFiatExchangeRateCollectionAsync(string baseCurrencySymbol);
        Task<decimal> ConvertAsync(string fromCurrencySymbol, string toCurrencySymbol, decimal amount);
        Task<decimal> GetUnitPriceAsync(string fromCurrencySymbol, string toCurrencySymbol);
    }
}
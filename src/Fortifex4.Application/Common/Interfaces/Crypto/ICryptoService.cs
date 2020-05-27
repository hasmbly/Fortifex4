using System.Threading.Tasks;

namespace Fortifex4.Application.Common.Interfaces.Crypto
{
    public interface ICryptoService
    {
        Task<CryptoBlockchainCollection> GetCryptoBlockchainCollectionAsync();
        Task<CryptoLatestQuotesResult> GetLatestQuoteAsync(string fromCurrencySymbol, string toCurrencySymbol);
        Task<decimal> ConvertAsync(string fromCurrencySymbol, string toCurrencySymbol, decimal amount);
        Task<decimal> GetUnitPriceAsync(string fromCurrencySymbol, string toCurrencySymbol);
    }
}
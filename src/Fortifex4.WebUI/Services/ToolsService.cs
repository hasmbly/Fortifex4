using System.Net.Http;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Currencies.Queries.GetPriceConversion;
using Fortifex4.Shared.Currencies.Queries.GetUnitPrice;
using Fortifex4.Shared.Currencies.Queries.GetUnitPriceInUSD;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IToolsService
    {
        public Task<ApiResponse<GetPriceConversionResponse>> GetPriceConversion(string fromCurrencySymbol, string toCurrencySymbol, decimal amount);
        public Task<ApiResponse<GetUnitPriceResponse>> GetUnitPrice(string fromCurrencySymbol, string toCurrencySymbol);
        public Task<ApiResponse<GetUnitPriceInUSDResponse>> GetUnitPriceInUSD(string currencySymbol);
    }

    public class ToolsService : IToolsService 
    {
        private readonly HttpClient _httpClient;

        public ToolsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<GetPriceConversionResponse>> GetPriceConversion(string fromCurrencySymbol, string toCurrencySymbol, decimal amount)
        {
            string queryParams = $"?fromCurrencySymbol={fromCurrencySymbol}?toCurrencySymbol={toCurrencySymbol}?amount={amount}";

            return await _httpClient.GetJsonAsync<ApiResponse<GetPriceConversionResponse>>(Constants.URI.Tools.GetPriceConversion + queryParams);
        }

        public async Task<ApiResponse<GetUnitPriceResponse>> GetUnitPrice(string fromCurrencySymbol, string toCurrencySymbol)
        {
            string queryParams = $"?fromCurrencySymbol={fromCurrencySymbol}?toCurrencySymbol={toCurrencySymbol}";

            return await _httpClient.GetJsonAsync<ApiResponse<GetUnitPriceResponse>>(Constants.URI.Tools.GetUnitPrice + queryParams);
        }

        public async Task<ApiResponse<GetUnitPriceInUSDResponse>> GetUnitPriceInUSD(string currencySymbol)
        {
            string queryParams = $"?currencySymbol={currencySymbol}";

            return await _httpClient.GetJsonAsync<ApiResponse<GetUnitPriceInUSDResponse>>(Constants.URI.Tools.GetUnitPriceInUSD + queryParams);
        }
    }
}
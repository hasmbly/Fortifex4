using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface ICurrenciesService
    {
        public Task<ApiResponse<GetAllFiatCurrenciesResponse>> GetAllFiatCurrencies();
        public Task<ApiResponse<GetPreferableCoinCurrenciesResponse>> GetPreferableCoinCurrencies();
    }

    public class CurrenciesService : ICurrenciesService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CurrenciesService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetAllFiatCurrenciesResponse>> GetAllFiatCurrencies()
        {
            await SetHeader();

            var getAllFiatCurrenciesResponse = await _httpClient.GetJsonAsync<ApiResponse<GetAllFiatCurrenciesResponse>>(Constants.URI.Currencies.GetAllFiatCurrencies);

            return getAllFiatCurrenciesResponse;
        }

        public async Task<ApiResponse<GetPreferableCoinCurrenciesResponse>> GetPreferableCoinCurrencies()
        {
            await SetHeader();

            var getPreferableCoinCurrenciesResponse = await _httpClient.GetJsonAsync<ApiResponse<GetPreferableCoinCurrenciesResponse>>(Constants.URI.Currencies.GetPreferableCoinCurrencies);

            return getPreferableCoinCurrenciesResponse;
        }
    }
}
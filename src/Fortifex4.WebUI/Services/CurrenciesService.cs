using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetCurrency;
using Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface ICurrenciesService
    {
        public Task<ApiResponse<GetCurrencyResponse>> GetCurrency(int currencyID);
        public Task<ApiResponse<GetAvailableCurrenciesResponse>> GetAvailableCurrencies(int ownerID);
        public Task<ApiResponse<GetDestinationCurrenciesForMemberResponse>> GetDestinationCurrenciesForMember(string memberUsername);
        public Task<ApiResponse<GetAllCoinCurrenciesResponse>> GetAllCoinCurrencies();


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

        public async Task<ApiResponse<GetCurrencyResponse>> GetCurrency(int currencyID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetCurrencyResponse>>($"{Constants.URI.Currencies.GetCurrency}/{currencyID}");
        }

        public async Task<ApiResponse<GetAvailableCurrenciesResponse>> GetAvailableCurrencies(int ownerID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetAvailableCurrenciesResponse>>($"{Constants.URI.Currencies.GetAvailableCurrencies}/{ownerID}");
        }

        public async Task<ApiResponse<GetDestinationCurrenciesForMemberResponse>> GetDestinationCurrenciesForMember(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetDestinationCurrenciesForMemberResponse>>($"{Constants.URI.Currencies.GetDestinationCurrenciesForMember}/{memberUsername}");
        }

        public async Task<ApiResponse<GetAllCoinCurrenciesResponse>> GetAllCoinCurrencies()
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetAllCoinCurrenciesResponse>>(Constants.URI.Currencies.GetAllCoinCurrencies);
        }
    }
}
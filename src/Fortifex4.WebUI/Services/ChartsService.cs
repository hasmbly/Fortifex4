using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Charts.Queries.GetCoinByExchanges;
using Fortifex4.Shared.Charts.Queries.GetPortfolioByExchanges;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IChartsService
    {
        public Task<ApiResponse<GetPortfolioResponse>> GetPortfolioByCoinsV2(string memberUsername);
        public Task<ApiResponse<GetPortfolioByExchangesResponse>> GetPortfolioByExchanges(string memberUsername);
        public Task<ApiResponse<GetCoinByExchangesResponse>> GetCoinByExchanges(string memberUsername, int currencyID);
    }

    public class ChartsService : IChartsService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ChartsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetPortfolioResponse>> GetPortfolioByCoinsV2(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetPortfolioResponse>>($"{Constants.URI.Charts.GetPortfolioByCoinsV2}/{memberUsername}");
        }

        public async Task<ApiResponse<GetPortfolioByExchangesResponse>> GetPortfolioByExchanges(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetPortfolioByExchangesResponse>>($"{Constants.URI.Charts.GetPortfolioByExchanges}/{memberUsername}");
        }

        public async Task<ApiResponse<GetCoinByExchangesResponse>> GetCoinByExchanges(string memberUsername, int currencyID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetCoinByExchangesResponse>>($"{Constants.URI.Charts.GetCoinByExchanges}/{memberUsername}/{currencyID}");
        }
    }
}
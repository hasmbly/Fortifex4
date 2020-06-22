using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Countries.Queries.GetAllCountries;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface ICountriesService
    {
        public Task<ApiResponse<GetAllCountriesResponse>> GetAllCountries();
    }

    public class CountriesService : ICountriesService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CountriesService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetAllCountriesResponse>> GetAllCountries()
        {
            await SetHeader();

            var getAllCountriesResponse = await _httpClient.GetJsonAsync<ApiResponse<GetAllCountriesResponse>>(Constants.URI.Countries.GetAllCountries);

            return getAllCountriesResponse;
        }
    }
}
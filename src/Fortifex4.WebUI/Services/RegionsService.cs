using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Regions.Queries.GetRegions;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IRegionsService
    {
        public Task<ApiResponse<GetRegionsResponse>> GetRegions(string countryCode);
    }

    public class RegionsService : IRegionsService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public RegionsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetRegionsResponse>> GetRegions(string countryCode)
        {
            await SetHeader();

            var getRegionsResponse = await _httpClient.GetJsonAsync<ApiResponse<GetRegionsResponse>>($"{Constants.URI.Regions.GetRegions}/{countryCode}");

            return getRegionsResponse;
        }
    }
}
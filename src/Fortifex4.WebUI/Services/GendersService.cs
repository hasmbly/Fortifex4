using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Genders.Queries.GetAllGenders;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IGendersService
    {
        public Task<ApiResponse<GetAllGendersResponse>> GetAllGenders();
    }

    public class GendersService : IGendersService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public GendersService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetAllGendersResponse>> GetAllGenders()
        {
            await SetHeader();

            var getAllGendersResponse = await _httpClient.GetJsonAsync<ApiResponse<GetAllGendersResponse>>(Constants.URI.Genders.GetAllGenders);

            return getAllGendersResponse;
        }
    }
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Common;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IBlockchainsService
    {
        public Task<ApiResponse<GetAllBlockchainsResponse>> GetAllBlockchains();
    }

    public class BlockchainsService : IBlockchainsService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public BlockchainsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetAllBlockchainsResponse>> GetAllBlockchains()
        {
            return await _httpClient.GetJsonAsync<ApiResponse<GetAllBlockchainsResponse>>(Constants.URI.Blockchains.GetAllBlockchains);
        }
    }
}
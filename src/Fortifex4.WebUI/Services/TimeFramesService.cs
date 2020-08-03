using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface ITimeFramesService
    {
        public Task<ApiResponse<GetAllTimeFramesResponse>> GetAllTimeFrames();
    }

    public class TimeFramesService : ITimeFramesService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public TimeFramesService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetAllTimeFramesResponse>> GetAllTimeFrames()
        {
            await SetHeader();

            ApiResponse<GetAllTimeFramesResponse> result = null;

            try
            {
                result = await _httpClient.GetJsonAsync<ApiResponse<GetAllTimeFramesResponse>>(Constants.URI.TimeFrames.GetAllTimeFrames);
            }
            catch (HttpRequestException e)
            {
                // if unauthorized 401 then redirect to login page
                System.Console.WriteLine($"HttpRequestException: {e.Message}");

                await ((ServerAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOutAsync();
            }

            return result;
        }
    }
}
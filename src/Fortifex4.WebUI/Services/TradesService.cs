using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Trades.Commands.CreateTrade;
using Fortifex4.Shared.Trades.Commands.DeleteTrade;
using Fortifex4.Shared.Trades.Commands.UpdateTrade;
using Fortifex4.Shared.Trades.Queries.GetTrade;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface ITradesService
    {
        public Task<ApiResponse<GetTradeResponse>> GetTrade(int tradeID);
        public Task<ApiResponse<CreateTradeResponse>> CreateTrade(CreateTradeRequest request);
        public Task<ApiResponse<UpdateTradeResponse>> UpdateTrade(UpdateTradeRequest request);
        public Task<ApiResponse<DeleteTradeResponse>> DeleteTrade(DeleteTradeRequest request);
    }

    public class TradesService : ITradesService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public TradesService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetTradeResponse>> GetTrade(int tradeID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetTradeResponse>>($"{Constants.URI.Trades.GetTrade}/{tradeID}");
        }

        public async Task<ApiResponse<CreateTradeResponse>> CreateTrade(CreateTradeRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateTradeResponse>>(Constants.URI.Trades.CreateTrade, request);
        }

        public async Task<ApiResponse<UpdateTradeResponse>> UpdateTrade(UpdateTradeRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateTradeResponse>>(Constants.URI.Trades.UpdateTrade, request);
        }

        public async Task<ApiResponse<DeleteTradeResponse>> DeleteTrade(DeleteTradeRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteTradeResponse>>(Constants.URI.Trades.DeleteTrade, request);
        }
    }
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IWalletsService
    {
        public Task<ApiResponse<CreatePersonalWalletResponse>> CreatePersonalWallet(CreatePersonalWalletRequest memberUsername);
        public Task<ApiResponse<GetPersonalWalletsResponse>> GetPersonalWallets(string memberUsername);
        public Task<ApiResponse<GetWalletResponse>> GetWallet(int walletID);
        public Task<ApiResponse<SyncPersonalWalletResponse>> SyncPersonalWallet(int walletID);
    }

    public class WalletsService : IWalletsService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public WalletsService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetPersonalWalletsResponse>> GetPersonalWallets(string memberUsername)
        {
            await SetHeader();

            var getPersonalWalletsResponse = await _httpClient.GetJsonAsync<ApiResponse<GetPersonalWalletsResponse>>($"{Constants.URI.Wallets.GetPersonalWallets}/{memberUsername}");

            return getPersonalWalletsResponse;
        }

        public async Task<ApiResponse<GetWalletResponse>> GetWallet(int walletID)
        {
            await SetHeader();

            var getWalletResponse = await _httpClient.GetJsonAsync<ApiResponse<GetWalletResponse>>($"{Constants.URI.Wallets.GetWallet}/{walletID}");

            return getWalletResponse;
        }

        public async Task<ApiResponse<SyncPersonalWalletResponse>> SyncPersonalWallet(int walletID)
        {
            await SetHeader();

            var syncPersonalWalletResponse = await _httpClient.GetJsonAsync<ApiResponse<SyncPersonalWalletResponse>>($"{Constants.URI.Wallets.SyncPersonalWallet}/{walletID}");

            return syncPersonalWalletResponse;
        }

        public async Task<ApiResponse<CreatePersonalWalletResponse>> CreatePersonalWallet(CreatePersonalWalletRequest request)
        {
            await SetHeader();

            var createPersonalWalletResponse = await _httpClient.PostJsonAsync<ApiResponse<CreatePersonalWalletResponse>>(Constants.URI.Wallets.CreatePersonalWallet, request);

            return createPersonalWalletResponse;
        }
    }
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IWalletsService
    {
        public Task<ApiResponse<CreatePersonalWalletResponse>> CreatePersonalWallet(CreatePersonalWalletRequest request);
        public Task<ApiResponse<UpdatePersonalWalletResponse>> UpdatePersonalWallet(UpdatePersonalWalletRequest request);
        public Task<ApiResponse<DeleteWalletResponse>> DeleteWallet(DeleteWalletRequest request);
        public Task<ApiResponse<GetPersonalWalletsResponse>> GetPersonalWallets(string memberUsername);
        public Task<ApiResponse<GetWalletResponse>> GetWallet(int walletID);
        public Task<ApiResponse<SyncPersonalWalletResponse>> SyncPersonalWallet(int walletID);
        public Task<ApiResponse<GetWalletsBySameUsernameAndBlockchainResponse>> GetWalletsWithSameCurrency(int walletID);
        public Task<ApiResponse<GetAllWalletsBySameUsernameAndBlockchainResponse>> GetAllWalletsWithSameCurrency(string memberUsername);
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

        public async Task<ApiResponse<UpdatePersonalWalletResponse>> UpdatePersonalWallet(UpdatePersonalWalletRequest request)
        {
            await SetHeader();

            var updatePersonalWalletResponse = await _httpClient.PutJsonAsync<ApiResponse<UpdatePersonalWalletResponse>>(Constants.URI.Wallets.UpdatePersonalWallet, request);

            return updatePersonalWalletResponse;
        }

        public async Task<ApiResponse<DeleteWalletResponse>> DeleteWallet(DeleteWalletRequest request)
        {
            await SetHeader();

            var deleteWalletResponse = await _httpClient.PostJsonAsync<ApiResponse<DeleteWalletResponse>>(Constants.URI.Wallets.DeleteWallet, request);

            return deleteWalletResponse;
        }

        public async Task<ApiResponse<GetWalletsBySameUsernameAndBlockchainResponse>> GetWalletsWithSameCurrency(int walletID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetWalletsBySameUsernameAndBlockchainResponse>>($"{Constants.URI.InternalTransfers.GetWalletsWithSameCurrency}/{walletID}");
        }

        public async Task<ApiResponse<GetAllWalletsBySameUsernameAndBlockchainResponse>> GetAllWalletsWithSameCurrency(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetAllWalletsBySameUsernameAndBlockchainResponse>>($"{Constants.URI.InternalTransfers.GetAllWalletsWithSameCurrency}/{memberUsername}");
        }
    }
}
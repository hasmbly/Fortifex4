using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain;
using Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IInternalTransfersService
    {
        public Task<ApiResponse<GetInternalTransferResponse>> GetInternalTransfer(int internalTransferID);
        public Task<ApiResponse<GetWalletsBySameUsernameAndBlockchainResponse>> GetWalletsWithSameCurrency(int walletID);
        public Task<ApiResponse<GetAllWalletsBySameUsernameAndBlockchainResponse>> GetAllWalletsWithSameCurrency(string memberUsername);
        public Task<ApiResponse<CreateInternalTransferResponse>> CreateInternalTransfer(CreateInternalTransferRequest request);
        public Task<ApiResponse<UpdateInternalTransferResponse>> UpdateInternalTransfer(UpdateInternalTransferRequest request);
        public Task<ApiResponse<DeleteInternalTransferResponse>> DeleteInternalTransfer(DeleteInternalTransferRequest request);
    }

    public class InternalTransfersService : IInternalTransfersService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public InternalTransfersService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetInternalTransferResponse>> GetInternalTransfer(int internalTransferID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetInternalTransferResponse>>($"{Constants.URI.InternalTransfers.GetInternalTransfer}/{internalTransferID}");
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

        public async Task<ApiResponse<CreateInternalTransferResponse>> CreateInternalTransfer(CreateInternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateInternalTransferResponse>>(Constants.URI.InternalTransfers.CreateInternalTransfer, request);
        }

        public async Task<ApiResponse<UpdateInternalTransferResponse>> UpdateInternalTransfer(UpdateInternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateInternalTransferResponse>>(Constants.URI.InternalTransfers.UpdateInternalTransfer, request);
        }

        public async Task<ApiResponse<DeleteInternalTransferResponse>> DeleteInternalTransfer(DeleteInternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteInternalTransferResponse>>(Constants.URI.InternalTransfers.DeleteInternalTransfer, request);
        }
    }
}
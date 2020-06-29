using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.ExternalTransfers.Commands.UpdateExternalTransfer;
using Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer;
using Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer;
using Fortifex4.Shared.Wallets.Commands.DeleteExternalTransfer;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IExternalTransfersService
    {
        public Task<ApiResponse<GetExternalTransferResponse>> GetExternalTransfer(int transactionID);
        public Task<ApiResponse<CreateExternalTransferResponse>> CreateExternalTransfer(CreateExternalTransferRequest request);
        public Task<ApiResponse<UpdateExternalTransferResponse>> UpdateExternalTransfer(UpdateExternalTransferRequest request);
        public Task<ApiResponse<DeleteExternalTransferResponse>> DeleteExternalTransfer(DeleteExternalTransferRequest request);
    }

    public class ExternalTransfersService : IExternalTransfersService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ExternalTransfersService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetExternalTransferResponse>> GetExternalTransfer(int transactionID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetExternalTransferResponse>>($"{Constants.URI.ExternalTransfers.GetExternalTransfer}/{transactionID}");
        }

        public async Task<ApiResponse<CreateExternalTransferResponse>> CreateExternalTransfer(CreateExternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateExternalTransferResponse>>(Constants.URI.ExternalTransfers.CreateExternalTransfer, request);
        }

        public async Task<ApiResponse<UpdateExternalTransferResponse>> UpdateExternalTransfer(UpdateExternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateExternalTransferResponse>>(Constants.URI.ExternalTransfers.UpdateExternalTransfer, request);
        }

        public async Task<ApiResponse<DeleteExternalTransferResponse>> DeleteExternalTransfer(DeleteExternalTransferRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteExternalTransferResponse>>(Constants.URI.ExternalTransfers.DeleteExternalTransfer, request);
        }
    }
}
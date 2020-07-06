using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Lookup.Queries.GetOwners;
using Fortifex4.Shared.Owners.Commands.CreateExchangeOwner;
using Fortifex4.Shared.Owners.Commands.DeleteOwner;
using Fortifex4.Shared.Owners.Commands.UpdateExchangeOwner;
using Fortifex4.Shared.Owners.Queries.GetExchangeOwners;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders;
using Fortifex4.Shared.Providers.Queries.GetProvider;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IOwnersService
    {
        public Task<ApiResponse<GetOwnerResponse>> GetOwner(int ownerID);
        public Task<ApiResponse<GetOwnersResponse>> GetOwners(string memberUsername);
        public Task<ApiResponse<GetProviderResponse>> GetProvider(int providerID);
        public Task<ApiResponse<GetExchangeOwnersResponse>> GetExchangeOwners(string memberUsername);
        public Task<ApiResponse<GetAvailableExchangeProvidersResponse>> GetAvailableExchangeProviders(string memberUsername);
        public Task<ApiResponse<CreateExchangeOwnerResponse>> CreateExchangeOwner(CreateExchangeOwnerRequest request);
        public Task<ApiResponse<UpdateExchangeOwnerResponse>> UpdateExchangeOwner(UpdateExchangeOwnerRequest request);
        public Task<ApiResponse<DeleteOwnerResponse>> DeleteOwner(DeleteOwnerRequest request);
    }

    public class OwnersService : IOwnersService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public OwnersService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetOwnerResponse>> GetOwner(int ownerID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetOwnerResponse>>($"{Constants.URI.Owners.GetOwner}/{ownerID}");
        }

        public async Task<ApiResponse<GetOwnersResponse>> GetOwners(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetOwnersResponse>>($"{Constants.URI.Owners.GetOwners}/{memberUsername}");
        }

        public async Task<ApiResponse<GetProviderResponse>> GetProvider(int providerID)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetProviderResponse>>($"{Constants.URI.Owners.GetProvider}/{providerID}");
        }

        public async Task<ApiResponse<CreateExchangeOwnerResponse>> CreateExchangeOwner(CreateExchangeOwnerRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<CreateExchangeOwnerResponse>>(Constants.URI.Owners.CreateExchangeOwner, request);
        }

        public async Task<ApiResponse<UpdateExchangeOwnerResponse>> UpdateExchangeOwner(UpdateExchangeOwnerRequest request)
        {
            await SetHeader();

            return await _httpClient.PutJsonAsync<ApiResponse<UpdateExchangeOwnerResponse>>(Constants.URI.Owners.UpdateExchangeOwner, request);
        }

        public async Task<ApiResponse<DeleteOwnerResponse>> DeleteOwner(DeleteOwnerRequest request)
        {
            await SetHeader();

            return await _httpClient.PostJsonAsync<ApiResponse<DeleteOwnerResponse>>(Constants.URI.Owners.DeleteOwner, request);
        }

        public async Task<ApiResponse<GetExchangeOwnersResponse>> GetExchangeOwners(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetExchangeOwnersResponse>>($"{Constants.URI.Owners.GetExchangeOwners}/{memberUsername}");
        }

        public async Task<ApiResponse<GetAvailableExchangeProvidersResponse>> GetAvailableExchangeProviders(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetAvailableExchangeProvidersResponse>>($"{Constants.URI.Owners.GetAvailableExchangeProviders}/{memberUsername}");
        }
    }
}
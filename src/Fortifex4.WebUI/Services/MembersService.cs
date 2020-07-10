using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using Fortifex4.Shared.Members.Commands.UpdatePreferredCoinCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.Shared.Members.Queries.GetPreferences;
using Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    public interface IMembersService
    {
        public Task<ApiResponse<GetMemberResponse>> GetMember(string MemberUsername);
        public Task<ApiResponse<UpdateMemberResponse>> UpdateMember(UpdateMemberRequest request);
        public Task<ApiResponse<GetPreferencesResponse>> GetPreferences(string MemberUsername);
        public Task<ApiResponse<UpdatePreferredTimeFrameResponse>> UpdatePreferredTimeFrame(UpdatePreferredTimeFrameRequest request);
        public Task<ApiResponse<UpdatePreferredCoinCurrencyResponse>> UpdatePreferredCoinCurrency(UpdatePreferredCoinCurrencyRequest request);
        public Task<ApiResponse<UpdatePreferredFiatCurrencyResponse>> UpdatePreferredFiatCurrency(UpdatePreferredFiatCurrencyRequest request);
        public Task<ApiResponse<GetTransactionsByMemberUsernameResponse>> GetTransactionsByMemberUsername(string memberUsername);
    }

    public class MembersService : IMembersService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public MembersService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task SetHeader()
        {
            string token = await ((ServerAuthenticationStateProvider)_authenticationStateProvider).GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Bearer, token);
        }

        public async Task<ApiResponse<GetMemberResponse>> GetMember(string MemberUsername)
        {
            await SetHeader();

            var loginResponse = await _httpClient.GetJsonAsync<ApiResponse<GetMemberResponse>>($"{Constants.URI.Account.GetMember}/{MemberUsername}");

            return loginResponse;
        }

        public async Task<ApiResponse<UpdateMemberResponse>> UpdateMember(UpdateMemberRequest request)
        {
            await SetHeader();

            var updateResponse = await _httpClient.PutJsonAsync<ApiResponse<UpdateMemberResponse>>(Constants.URI.Members.UpdateMember, request);

            return updateResponse;
        }

        public async Task<ApiResponse<GetPreferencesResponse>> GetPreferences(string MemberUsername)
        {
            await SetHeader();

            var getPreferencesResponse = await _httpClient.GetJsonAsync<ApiResponse<GetPreferencesResponse>>($"{Constants.URI.Members.GetPreferences}/{MemberUsername}");

            return getPreferencesResponse;
        }

        public async Task<ApiResponse<UpdatePreferredTimeFrameResponse>> UpdatePreferredTimeFrame(UpdatePreferredTimeFrameRequest request)
        {
            await SetHeader();

            var updatePreferredTimeFrameResponse = await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredTimeFrameResponse>>(Constants.URI.Members.UpdatePreferredTimeFrame, request);

            return updatePreferredTimeFrameResponse;
        }

        public async Task<ApiResponse<UpdatePreferredCoinCurrencyResponse>> UpdatePreferredCoinCurrency(UpdatePreferredCoinCurrencyRequest request)
        {
            await SetHeader();

            var updatePreferredCoinCurrencyResponse = await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredCoinCurrencyResponse>>(Constants.URI.Members.UpdatePreferredCoinCurrency, request);

            return updatePreferredCoinCurrencyResponse;
        }

        public async Task<ApiResponse<UpdatePreferredFiatCurrencyResponse>> UpdatePreferredFiatCurrency(UpdatePreferredFiatCurrencyRequest request)
        {
            await SetHeader();

            var updatePreferredFiatCurrencyResponse = await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredFiatCurrencyResponse>>(Constants.URI.Members.UpdatePreferredFiatCurrency, request);

            return updatePreferredFiatCurrencyResponse;
        }

        public async Task<ApiResponse<GetTransactionsByMemberUsernameResponse>> GetTransactionsByMemberUsername(string memberUsername)
        {
            await SetHeader();

            return await _httpClient.GetJsonAsync<ApiResponse<GetTransactionsByMemberUsernameResponse>>($"{Constants.URI.Members.GetTransactionsByMemberUsername}/{memberUsername}");
        }
    }
}
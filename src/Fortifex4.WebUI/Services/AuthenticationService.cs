using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Commands.ActivateMember;
using Fortifex4.Shared.Members.Commands.CreateMember;
using Fortifex4.Shared.Members.Queries.Login;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    interface IAuthenticationService
    {
        public Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
        public Task<ApiResponse<CreateMemberResponse>> Register(CreateMemberRequest request);
        public Task<ApiResponse<ActivateMemberResponse>> ActivateMember(Guid ActivationCode);
        public Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest request)
        {
            var loginResponse = await _httpClient.PostJsonAsync<ApiResponse<LoginResponse>>(Constants.URI.Account.Login, request);

            if (loginResponse.Result.IsSuccessful)
                await ((ServerAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticatedAsync(loginResponse.Result.Token);

            return loginResponse;
        }

        public async Task<ApiResponse<CreateMemberResponse>> Register(CreateMemberRequest request)
        {
            var createMemberResponse = await _httpClient.PostJsonAsync<ApiResponse<CreateMemberResponse>>(Constants.URI.Members.CreateMember, request);

            return createMemberResponse;
        }

        public async Task<ApiResponse<ActivateMemberResponse>> ActivateMember(Guid ActivationCode)
        {
            var activateMemberResponse = await _httpClient.GetJsonAsync<ApiResponse<ActivateMemberResponse>>(Constants.URI.Account.ActivateMember + ActivationCode);

            return activateMemberResponse;
        }

        public async Task Logout()
        {
            await ((ServerAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOutAsync();
        }
    }
}
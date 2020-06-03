using System.Threading.Tasks;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Services
{
    interface IAuthenticationService
    {
        public Task Login(string token);
        public Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task Login(string token)
        {
            await ((ServerAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticatedAsync(token);
        }

        public async Task Logout()
        {
            await ((ServerAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOutAsync();
        }
    }
}
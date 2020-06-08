using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class TopBar
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override void OnInitialized()
        {
        }

        private void PrefCoinSelected(ChangeEventArgs e)
        {
            if (int.TryParse((string)e.Value, out var index) && index >= 0)
            {
                //AddTopping(toppings[index]);
            }
        }

        private async Task LogUsername()
        {
            var authenticationState = await AuthenticationStateTask;
            var user = authenticationState.User;

            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;

                foreach (var claim in claimsIdentity.Claims)
                {
                    Console.WriteLine(claim.Type + ":" + claim.Value);
                }
            }
            else
            {
                Console.WriteLine("The user is NOT authenticated.");
            }
        }

        private async Task LogoutAsync()
        {
            Console.WriteLine("You pressed logout.");

            await _authenticationService.Logout();

            _navigationManager.NavigateTo("/");
        }
    }
}
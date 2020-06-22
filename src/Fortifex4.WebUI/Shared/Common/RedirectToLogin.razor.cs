using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class RedirectToLogin
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateTask;

            if (authenticationState?.User?.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
            {
                var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);

                if (string.IsNullOrWhiteSpace(returnUrl))
                    _navigationManager.NavigateTo("account/login", true);
                else
                    _navigationManager.NavigateTo($"account/login?returnUrl={returnUrl}", true);
            }
        }
    }
}
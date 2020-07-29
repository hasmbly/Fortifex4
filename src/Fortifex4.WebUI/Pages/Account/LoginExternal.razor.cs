using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class LoginExternal
    {
        public string Token { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var query = new Uri(_navigationManager.Uri).Query;

            if (QueryHelpers.ParseQuery(query).TryGetValue("token", out var value))
            {
                Token = value;

                await _authenticationService.LoginExternal(this.Token);
                _navigationManager.NavigateTo("/portfolio");
            }
        }
    }
}
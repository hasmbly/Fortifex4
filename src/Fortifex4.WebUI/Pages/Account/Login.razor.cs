using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Login
    {
        private void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
            {
                // check if has "requestUri"
                var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);

                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var param))
                {
                    _navigationManager.NavigateTo($"/{param.First()}");
                }
                else
                {
                    _navigationManager.NavigateTo("/portfolio");
                }
            }
        }
    }
}
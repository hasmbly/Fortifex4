namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Login
    {
        private void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                _navigationManager.NavigateTo("/portfolio");
        }
    }
}
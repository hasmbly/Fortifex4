namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Register
    {
        protected override void OnInitialized()
        {
            activateMemberState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            activateMemberState.OnChange -= StateHasChanged;
        }
    }
}
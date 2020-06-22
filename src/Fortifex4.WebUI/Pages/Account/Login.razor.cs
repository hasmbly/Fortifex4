using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.Login;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Login
    {
        public string Message { get; set; }

        public bool IsLoading { get; set; }
        
        public LoginRequest Input { get; set; } = new LoginRequest();

        protected async override Task OnInitializedAsync()
        {
            await _authenticationService.Logout();
        }

        private async Task LoginAsync()
        {
            this.Message = string.Empty;

            this.IsLoading = true;

            var loginResponse = await _authenticationService.Login(this.Input);

            if (loginResponse.Status.IsError)
            {
                this.Message = loginResponse.Status.Message;

                this.IsLoading = false;
            }
            else
            {
                if (loginResponse.Result.IsSuccessful)
                {
                    activateMemberState.ResetActivateMemberState();

                    _navigationManager.NavigateTo("/portfolio");
                }
                else
                {
                    this.Message = loginResponse.Result.ErrorMessage;

                    this.IsLoading = false;
                }
            }
        }
    }
}
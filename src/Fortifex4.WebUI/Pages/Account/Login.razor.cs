using System;
using System.Text.Json;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Queries.Login;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Login
    {
        public string Message { get; set; }
        public LoginRequest Input { get; set; } = new LoginRequest();

        protected async override Task OnInitializedAsync()
        {
            await _authenticationService.Logout();
        }

        private async Task LoginAsync()
        {
            this.Message = string.Empty;

            var loginResponse = await _httpClient.PostJsonAsync<ApiResponse<LoginResponse>>(Constants.URI.Account.Login, this.Input);

            Console.WriteLine(JsonSerializer.Serialize(loginResponse));

            if (loginResponse.Status.IsError)
            {
                this.Message = loginResponse.Status.Message;
            }
            else
            {
                if (loginResponse.Result.IsSuccessful)
                {
                    await _authenticationService.Login(loginResponse.Result.Token);

                    activateMemberState.ResetActivateMemberState();

                    _navigationManager.NavigateTo("/portfolio");
                }
                else
                {
                    this.Message = loginResponse.Result.ErrorMessage;
                }
            }
        }
    }
}
using System.Threading.Tasks;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Members.Queries.Login;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class SignIn
    {
        [Parameter]
        public bool IsRegisterProjectState { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public string Message { get; set; }

        public bool IsLoading { get; set; }

        public LoginRequest Input { get; set; } = new LoginRequest();

        public string MemberUsername
        {
            get => Input.MemberUsername;
            set
            {
                Input.MemberUsername = value;

                CheckIsEsistMemberUsername(value);
            }
        }

        protected async override Task OnInitializedAsync()
        {
            await _authenticationService.Logout();

            if (!string.IsNullOrEmpty(_projectState.ExistMemberUsername))
                MemberUsername = _projectState.ExistMemberUsername;
        }

        private async Task LoginAsync()
        {
            Message = string.Empty;

            IsLoading = true;

            var loginResponse = await _authenticationService.Login(this.Input);

            if (loginResponse.Status.IsError)
            {
                Message = loginResponse.Status.Message;

                IsLoading = false;
            }
            else
            {
                if (loginResponse.Result.IsSuccessful)
                {
                    activateMemberState.ResetActivateMemberState();

                    if (IsRegisterProjectState)
                    {
                        _projectState.SetIsAuthenticated(true);
                        _projectState.SetMessage("primary", $"Signed as {Input.MemberUsername}");
                    }

                    await OnAfterSuccessful.InvokeAsync(true);
                }
                else
                {
                    Message = loginResponse.Result.ErrorMessage;

                    IsLoading = false;
                }
            }
        }

        private async void CheckIsEsistMemberUsername(string _memberUsername)
        {
            var checkUsername = await _authenticationService.CheckUsername(_memberUsername);

            if (!checkUsername.Result.IsSuccessful)
            {
                Message = ErrorMessage.MemberUsernameNotFound;

                _memberUsername = string.Empty;

                if (IsRegisterProjectState)
                    _projectState.SetExistMemberUsername(_memberUsername);
            }
        }
    }
}
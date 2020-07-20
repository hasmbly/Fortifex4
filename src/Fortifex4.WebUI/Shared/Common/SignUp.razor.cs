using System.Threading.Tasks;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Members.Commands.CreateMember;
using Fortifex4.Shared.Members.Queries.Login;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class SignUp
    {
        [Parameter]
        public bool IsRegisterProjectState { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public bool IsLoading { get; set; }
        public string Message { get; set; }
        public string ConfirmPassword { get; set; }

        public CreateMemberRequest Input { get; set; } = new CreateMemberRequest();

        public string MemberUsername
        {
            get => Input.MemberUsername;
            set
            {
                Input.MemberUsername = value;

                CheckIsEsistMemberUsername(value);
            }
        }

        private async Task RegisterAsync()
        {
            Message = string.Empty;

            IsLoading = true;

            if (ConfirmPassword != Input.Password)
            {
                Message = ErrorMessage.PasswordDoNotMatch;

                IsLoading = false;
            }
            else
            {
                var createMemberResponse = await _authenticationService.Register(Input);

                if (createMemberResponse.Status.IsError)
                {
                    Message = createMemberResponse.Status.Message;

                    IsLoading = false;
                }
                else
                {
                    if (createMemberResponse.Result.IsSuccessful)
                    {
                        IsLoading = false;

                        if (IsRegisterProjectState)
                        {
                            await _authenticationService.ActivateMember(createMemberResponse.Result.ActivationCode);
                            await _authenticationService.Login(new LoginRequest() { MemberUsername = Input.MemberUsername, Password = Input.Password });

                            _projectState.SetIsAuthenticated(true);
                            _projectState.SetMessage("primary", $"Signed as {Input.MemberUsername}");

                            await OnAfterSuccessful.InvokeAsync(true);
                        }
                        else
                        {
                            activateMemberState.SetActivateMemberState(createMemberResponse.Result);
                        }
                    }
                    else
                    {
                        Message = createMemberResponse.Result.ErrorMessage;

                        IsLoading = false;
                    }
                }
            }
        }

        private async void CheckIsEsistMemberUsername(string _memberUsername)
        {
            var checkUsername = await _authenticationService.CheckUsername(_memberUsername);

            if (checkUsername.Result.DoesMemberExist)
            {
                Message = ErrorMessage.MemberUsernameAlreadyTaken;

                if (IsRegisterProjectState)
                    _projectState.SetExistMemberUsername(_memberUsername);
            }
        }
    }
}
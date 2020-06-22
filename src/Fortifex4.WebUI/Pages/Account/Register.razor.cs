using System.Threading.Tasks;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Members.Commands.CreateMember;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Register
    {
        public string Message { get; set; }

        public bool IsLoading { get; set; }
        
        public string ConfirmPassword { get; set; }
        
        public CreateMemberRequest Input { get; set; } = new CreateMemberRequest();

        protected override void OnInitialized()
        {
        }

        private async Task RegisterAsync()
        {
            IsLoading = true;

            if (ConfirmPassword != this.Input.Password)
            {
                Message = ErrorMessage.PasswordDoNotMatch;

                IsLoading = false;
            }
            else
            {
                var createMemberResponse = await _authenticationService.Register(this.Input);

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

                        activateMemberState.SetActivateMemberState(createMemberResponse.Result);

                        activateMemberState.OnChange += StateHasChanged;
                    }
                    else
                    {
                        Message = createMemberResponse.Result.ErrorMessage;

                        IsLoading = false;
                    }
                }
            }
        }
    }
}
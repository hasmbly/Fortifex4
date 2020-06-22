using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class RegistrationSucceed
    {
        public string ActivationURL { get; set; }

        public bool IsLoading { get; set; }

        private async Task Activate()
        {
            IsLoading = true;

            var MemberActivation = await _authenticationService.ActivateMember(activateMemberState.Member.ActivationCode);

            if (MemberActivation.Status.IsError)
            {
                IsLoading = false;

                Console.WriteLine(JsonSerializer.Serialize(MemberActivation.Status.IsError));
            }
            else
            {
                if (MemberActivation.Result.IsSuccessful)
                {
                    IsLoading = false;

                    activateMemberState.DoneActivateMemberState();

                    activateMemberState.OnChange += StateHasChanged;
                }
            }
        }
    }
}
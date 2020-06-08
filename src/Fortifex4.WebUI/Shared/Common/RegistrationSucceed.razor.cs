using System;
using System.Text.Json;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Commands.ActivateMember;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class RegistrationSucceed
    {
        public string ActivationURL { get; set; }

        private async Task Activate()
        {
            ActivationURL = Constants.URI.Account.ActivateMember + appState.Member.ActivationCode;

            var MemberActivation = await _httpClient.GetJsonAsync<ApiResponse<ActivateMemberResponse>>(ActivationURL);

            Console.WriteLine(JsonSerializer.Serialize(MemberActivation));

            if (MemberActivation.Status.IsError)
            {
                Console.WriteLine(JsonSerializer.Serialize(MemberActivation.Status.IsError));
            }
            else
            {
                if (MemberActivation.Result.IsSuccessful)
                {
                    appState.DoneActivateMemberState();

                    appState.OnChange += StateHasChanged;

                    Console.WriteLine($"Activation OnInitialized GetIsNeedActivation: " + appState.Member.ActivationCode);

                    //_navigationManager.NavigateTo("/account/login");
                }
            }
        }
    }
}
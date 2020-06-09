using System;
using System.Text.Json;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Commands.CreateMember;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Register
    {
        public string Message { get; set; }
        public string ConfirmPassword { get; set; }
        public CreateMemberRequest Input { get; set; } = new CreateMemberRequest();

        protected override void OnInitialized()
        {
        }

        private async Task RegisterAsync()
        {
            if (ConfirmPassword != this.Input.Password)
            {
                Message = "The password and confirmation password do not match.";
            }
            else
            {
                var createMemberResponse = await _httpClient.PostJsonAsync<ApiResponse<CreateMemberResponse>>(Constants.URI.Members.CreateMember, this.Input);

                Console.WriteLine(JsonSerializer.Serialize(createMemberResponse));

                if (createMemberResponse.Status.IsError)
                {
                    Message = createMemberResponse.Status.Message;
                }
                else
                {
                    if (createMemberResponse.Result.IsSuccessful)
                    {
                        activateMemberState.SetActivateMemberState(createMemberResponse.Result);

                        activateMemberState.OnChange += StateHasChanged;

                        Console.WriteLine($"Activate Member OnInitialized GetActivationCode(): " + activateMemberState.Member.ActivationCode);
                    }
                    else
                    {
                        Message = createMemberResponse.Result.ErrorMessage;
                    }
                }
            }
        }
    }
}
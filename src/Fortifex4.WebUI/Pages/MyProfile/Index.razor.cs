using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages.MyProfile
{
    public partial class Index
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public GetMemberResponse Member { get; set; } = new GetMemberResponse();

        public ClaimsPrincipal User { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _httpClient = ((ServerAuthenticationStateProvider)_authenticationStateProvider).Client();

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _httpClient.GetJsonAsync<ApiResponse<GetMemberResponse>>($"{Constants.URI.Account.GetMember}/{User.Identity.Name}");

            Member = result.Result;
        }
    }
}
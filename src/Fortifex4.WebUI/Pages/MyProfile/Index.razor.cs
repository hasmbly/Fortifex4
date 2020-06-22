using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.WebUI.Shared.Common.Modal;
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

        public bool IsLoading { get; set; }

        private ModalEditMyProfile ModalEditMyProfile { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        private async void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _membersService.GetMember(User.Identity.Name);

            if (result.Result.IsSuccessful)
                IsLoading = false;
                Member = result.Result;

            StateHasChanged();
        }
    }
}
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Owners.Queries.GetExchangeOwners;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages.Exchanges
{
    public partial class Index
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        public bool LoadPockets { get; set; } = true;

        public GetExchangeOwnersResponse GetExchangeOwnersResponse { get; set; } = new GetExchangeOwnersResponse();

        private ModalCreateExchange ModalCreateExchange { get; set; }

        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync()
        {
            globalState.ShouldRender += RefreshMe;

            await InitAsync();
        }

        public void Dispose()
        {
            globalState.ShouldRender -= RefreshMe;
        }

        private async void RefreshMe()
        {
            await InitAsync();
        }

        private async void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _ownersService.GetExchangeOwners(User.Identity.Name);

            if (result.Result.IsSuccessful)
                GetExchangeOwnersResponse = result.Result;
            
            IsLoading = false;

            StateHasChanged();
        }
    }
}
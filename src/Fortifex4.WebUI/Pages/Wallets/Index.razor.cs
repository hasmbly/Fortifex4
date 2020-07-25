using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Wallets.Queries.GetMyPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages.Wallets
{
    public partial class Index
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        public GetMyPersonalWalletsResponse GetMyPersonalWalletsResponse { get; set; } = new GetMyPersonalWalletsResponse();
        public GetPersonalWalletsResponse GetPersonalWalletsResponse { get; set; } = new GetPersonalWalletsResponse();

        private ModalCreatePersonalWallet ModalCreatePersonalWallet { get; set; }

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

            var result = await _walletsService.GetPersonalWallets(User.Identity.Name);

            if (result.Result.IsSuccessful)
                GetPersonalWalletsResponse = result.Result;
            IsLoading = false;

            StateHasChanged();
        }

        //private async Task InitAsync()
        //{
        //    IsLoading = true;

        //    User = Task.FromResult(await AuthenticationStateTask).Result.User;

        //    var result = await _walletsService.GetMyPersonalWallets();

        //    if (result.Result.IsSuccessful)
        //        GetMyPersonalWalletsResponse = result.Result;
        //    IsLoading = false;

        //    StateHasChanged();
        //}
    }
}
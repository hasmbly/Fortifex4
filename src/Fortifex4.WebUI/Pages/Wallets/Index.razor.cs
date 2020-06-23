using System.Security.Claims;
using System.Threading.Tasks;
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

        public GetPersonalWalletsResponse GetPersonalWalletsResponse { get; set; } = new GetPersonalWalletsResponse();

        private ModalCreatePersonalWallet ModalCreatePersonalWallet { get; set; }

        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync()
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
    }
}
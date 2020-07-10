using System.Threading.Tasks;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Pages.Exchanges
{
    public partial class Details
    {
        [Parameter]
        public int OwnerID { get; set; }

        public GetOwnerResponse Owner { get; set; } = new GetOwnerResponse();

        private ModalCreateTrade ModalCreateTrade { get; set; }
        private ModalCreateExchangeWallet ModalCreateExchangeWallet { get; set; }

        private ModalEditExchange ModalEditExchange { get; set; }
        private ModalDeleteExchange ModalDeleteExchange { get; set; }

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

            var result = await _ownersService.GetOwner(OwnerID);

            if (result.Result.IsSuccessful)
                Owner = result.Result;
            
            IsLoading = false;

            StateHasChanged();
        }
    }
}
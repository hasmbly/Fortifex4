using System.Threading.Tasks;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages.Exchanges
{
    public partial class Details
    {
        [Parameter]
        public int OwnerID { get; set; }

        public GetOwnerResponse Owner { get; set; } = new GetOwnerResponse();

        private ModalEditExchange ModalEditExchange { get; set; }
        private ModalDeleteExchange ModalDeleteExchange { get; set; }

        private ModalCreateTrade ModalCreateTrade { get; set; }

        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("Toggle.init");
            }
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
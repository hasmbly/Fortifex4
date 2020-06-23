using System.Threading.Tasks;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Pages.Wallets
{
    public partial class Details
    {
        [Parameter]
        public int WalletID { get; set; }

        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();

        private ModalEditPersonalWallet ModalEditPersonalWallet { get; set; }

        private ModalDeletePersonalWallet ModalDeletePersonalWallet { get; set; }

        public string SyncMessage { get; set; }
        
        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        public async void SyncWallet()
        {
            SyncMessage = string.Empty;

            try
            {
                var result = await _walletsService.SyncPersonalWallet(WalletID);

                if (result.Result.IsSuccessful)
                {
                    SyncMessage = "Synchronization process completed successfully";

                    await InitAsync();
                }
            }
            catch (InvalidWalletAddressException iwaex)
            {
                SyncMessage = iwaex.Message;
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

            var result = await _walletsService.GetWallet(WalletID);
            
            if (result.Result.IsSuccessful)
                Wallet = result.Result;
                IsLoading = false;

            StateHasChanged();
        }
    }
}
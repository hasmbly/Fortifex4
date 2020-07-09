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

        private ModalCreateExternalTransfer ModalCreateExternalTransfer { get; set; }
        private ModalCreateInternalTransfer ModalCreateInternalTransfer { get; set; }

        public string SyncMessage { get; set; }
        
        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        public async void SyncWallet()
        {
            IsLoading = true;

            SyncMessage = string.Empty;

            try
            {
                var result = await _walletsService.SyncPersonalWallet(WalletID);
                
                if (result.Status.IsError)
                {
                    SyncMessage = "There's a problem in Synchronization process";

                    StateHasChanged();
                }
                else
                {
                    if (result.Result.IsSuccessful)
                    {
                        await InitAsync();
                        SyncMessage = "Synchronization process completed successfully";

                        IsLoading = false;

                        StateHasChanged();
                    }
                    else
                    {
                        SyncMessage = "There's a problem in Synchronization process";

                        StateHasChanged();
                    }
                }
            }
            catch (InvalidWalletAddressException iwaex)
            {
                SyncMessage = iwaex.Message;

                StateHasChanged();
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
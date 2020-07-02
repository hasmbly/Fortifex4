﻿using System.Threading.Tasks;
using Fortifex4.Domain.Exceptions;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages.Wallets
{
    public partial class Details
    {
        [Parameter]
        public int WalletID { get; set; }

        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();

        private ModalEditPersonalWallet ModalEditPersonalWallet { get; set; }
        private ModalEditImportBalance ModalEditImportBalance { get; set; }
        private ModalEditExternalTransfer ModalEditExternalTransfer { get; set; }
        private ModalEditInternalTransfer ModalEditInternalTransfer { get; set; }
        private ModalDeletePersonalWallet ModalDeletePersonalWallet { get; set; }
        private ModalCreateInternalTransfer ModalCreateInternalTransfer { get; set; }
        private ModalCreateExternalTransfer ModalCreateExternalTransfer { get; set; }

        public string SyncMessage { get; set; }
        
        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("Toggle.init");
            }
        }

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

                    System.Console.WriteLine($"IsError: {result.Status.Message}");

                    StateHasChanged();
                }
                else
                {
                    if (result.Result.IsSuccessful)
                    {
                        await InitAsync();
                        SyncMessage = "Synchronization process completed successfully";

                        StateHasChanged();

                        IsLoading = false;
                    }
                    else
                    {
                        SyncMessage = "There's a problem in Synchronization process";

                        System.Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");

                        StateHasChanged();
                    }
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
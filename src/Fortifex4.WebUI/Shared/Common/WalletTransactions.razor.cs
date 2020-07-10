using System.Threading.Tasks;
using Fortifex4.Shared.Pockets.Queries.GetPocket;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class WalletTransactions
    {
        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();

        [Parameter]
        public GetPocketResponse Pocket { get; set; } = new GetPocketResponse();

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public string TransactionsTableID { get; set; } = "data-table-transactions";

        private ModalEditTrade ModalEditTrade { get; set; }
        private ModalEditImportBalance ModalEditImportBalance { get; set; }
        private ModalEditExternalTransfer ModalEditExternalTransfer { get; set; }
        private ModalEditInternalTransfer ModalEditInternalTransfer { get; set; }

        private ModalDeleteTrade ModalDeleteTrade { get; set; }
        private ModalDeleteExternalTransfer ModalDeleteExternalTransfer { get; set; }
        private ModalDeleteInternalTransfer ModalDeleteInternalTransfer { get; set; }

        protected override void OnInitialized()
        {
            System.Console.WriteLine($"WalletTransactions - OnInitialized");

            FirstStage = true;
        }

        private async void InvokeSuccessful()
        {
            await OnAfterSuccessful.InvokeAsync(true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            System.Console.WriteLine($"WalletTransactions - OnAfterRender");

            if (firstRender)
            {
                System.Console.WriteLine($"WalletTransactions - OnAfterRender - firstRender");
            }
            else if (FirstStage)
            {
                FirstStage = false;

                System.Console.WriteLine($"WalletTransactions - OnAfterRender - init DataTable");

                await JsRuntime.InvokeVoidAsync("DataTable.init", $"#{TransactionsTableID}");
            }
        }
    }
}
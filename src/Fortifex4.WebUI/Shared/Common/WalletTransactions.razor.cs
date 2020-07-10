using Fortifex4.Shared.Pockets.Queries.GetPocket;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;

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

        private ModalEditTrade ModalEditTrade { get; set; }
        private ModalEditImportBalance ModalEditImportBalance { get; set; }
        private ModalEditExternalTransfer ModalEditExternalTransfer { get; set; }
        private ModalEditInternalTransfer ModalEditInternalTransfer { get; set; }

        private ModalDeleteTrade ModalDeleteTrade { get; set; }
        private ModalDeleteExternalTransfer ModalDeleteExternalTransfer { get; set; }
        private ModalDeleteInternalTransfer ModalDeleteInternalTransfer { get; set; }

        private async void InvokeSuccessful()
        {
            await OnAfterSuccessful.InvokeAsync(true);
        }
    }
}
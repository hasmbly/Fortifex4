using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages
{
    public partial class Transactions
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public string TransactionsTableID { get; set; } = "data-table-transactions";

        public bool IsLoading { get; set; }
        
        public ClaimsPrincipal User { get; set; }

        public GetTransactionsByMemberUsernameResponse _Transactions { get; set; } = new GetTransactionsByMemberUsernameResponse();

        private ModalCreateTrade ModalCreateTrade { get; set; }
        private ModalEditTrade ModalEditTrade { get; set; }
        private ModalDeleteTrade ModalDeleteTrade { get; set; }

        private ModalEditSync ModalEditSync { get; set; }
        private ModalEditStartingBalance ModalEditStartingBalance { get; set; }

        private ModalCreateExternalTransfer ModalCreateExternalTransfer { get; set; }
        private ModalEditExternalTransfer ModalEditExternalTransfer { get; set; }
        private ModalDeleteExternalTransfer ModalDeleteExternalTransfer { get; set; }

        private ModalCreateInternalTransfer ModalCreateInternalTransfer { get; set; }
        private ModalEditInternalTransfer ModalEditInternalTransfer { get; set; }
        private ModalDeleteInternalTransfer ModalDeleteInternalTransfer { get; set; }

        private ModalEditImportBalance ModalEditImportBalance { get; set; }

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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) { }

            else if (FirstStage)
            {
                await JsRuntime.InvokeVoidAsync("DataTable.init", $"#{TransactionsTableID}");
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

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _membersService.GetTransactionsByMemberUsername(User.Identity.Name);

            _Transactions = result.Result;

            IsLoading = false;

            StateHasChanged();

            FirstStage = true;
        }
    }
}
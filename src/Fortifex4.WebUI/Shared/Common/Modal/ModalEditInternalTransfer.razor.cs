using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer;
using Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditInternalTransfer
    {
        public string Title { get; set; } = "Edit Internal Transfer";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int? WalletID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; }

        public UpdateInternalTransferRequest Input { get; set; } = new UpdateInternalTransferRequest();

        public GetInternalTransferResponse Wallet { get; set; } = new GetInternalTransferResponse();

        protected async override Task OnInitializedAsync()
        {
            User = Task.FromResult(await AuthenticationStateTask).Result.User;
        }

        private async Task LoadDataAsync(int internalTransferID)
        {
            IsLoading = true;

            var getWalletInternalTransfer = await _internalTransfersService.GetInternalTransfer(internalTransferID);
            Wallet = getWalletInternalTransfer.Result;

            Input.InternalTransferID = internalTransferID;
            LoadExistingData();

            IsLoading = false;

            StateHasChanged();
        }

        private void LoadExistingData()
        {
            Input.Amount = ToFixed4(Wallet.TransactionAmount);
            Input.TransactionDateTime = Wallet.TransactionDateTime;
        }

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private async void OnSubmitEditInternalTransferAsync()
        {
            StateHasChanged();

            IsLoading = true;

            var result = await _internalTransfersService.UpdateInternalTransfer(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    IsLoading = false;

                    await OnAfterSuccessful.InvokeAsync(true);

                    BaseModal.Close();
                }
                else
                {
                    System.Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
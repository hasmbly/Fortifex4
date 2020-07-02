using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Sync.Commands.UpdateSync;
using Fortifex4.Shared.Sync.Queries.GetSync;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditImportBalance
    {
        public string Title { get; set; } = "Edit Import Balance";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public bool IsLoading { get; set; }

        public decimal Total { get; set; }

        public BaseModal BaseModal { get; set; }

        public UpdateSyncRequest Input { get; set; } = new UpdateSyncRequest();

        public GetSyncResponse Wallet { get; set; } = new GetSyncResponse();

        public decimal UnitPriceInUSD
        {
            get => Input.UnitPriceInUSD;
            set
            {
                Input.UnitPriceInUSD = ToFixed4(value);

                CalculateAmount();
            }
        }

        protected async override Task OnInitializedAsync()
        {
            User = Task.FromResult(await AuthenticationStateTask).Result.User;
        }

        private async Task LoadDataAsync(int transactionID)
        {
            IsLoading = true;

            var getWallet = await _walletsService.GetDetailsSyncPersonalWallet(transactionID);
            Wallet = getWallet.Result;

            Input.TransactionID = transactionID;
            Input.UnitPriceInUSD = decimal.Parse(Wallet.UnitPriceInUSD.ToString("N4").Replace(".0000", ""));

            IsLoading = false;

            CalculateAmount();
        }

        private void CalculateAmount()
        {
            Total = Wallet.Amount * Input.UnitPriceInUSD;

            StateHasChanged();
        }

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private async void OnSubmitEditImportBalanceAsync()
        {
            IsLoading = true;

            var result = await _walletsService.UpdateDetailsSyncPersonalWallet(Input);

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
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
                    Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
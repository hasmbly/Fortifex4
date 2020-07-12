using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.StartingBalance.Commands.UpdateStartingBalance;
using Fortifex4.Shared.StartingBalance.Queries.GetStartingBalance;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static Fortifex4.WebUI.Shared.Common.ToggleCheckbox;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditStartingBalance
    {

        public string Title { get; set; } = "Edit Starting Balance";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int? WalletID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; }
        public bool IsPublic { get; set; }
        public decimal Total { get; set; }

        public const string ToggleCheckboxElementID = "edit-starting-balance";

        public UpdateStartingBalanceRequest Input { get; set; } = new UpdateStartingBalanceRequest();

        public GetStartingBalanceResponse Wallet { get; set; } = new GetStartingBalanceResponse();

        public ToggleCheckboxAttributes Attributes { get; set; } = new ToggleCheckboxAttributes
            (new ToggleCheckboxAttributesValue
            {
                ElementID = ToggleCheckboxElementID,
                DataOn = "Starting Balance"
            });

        public decimal Amount
        {
            get => Input.Amount;
            set
            {
                Input.Amount = ToFixed4(value);

                CalculateAmount();
            }
        }

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

            _toggleCheckboxState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _toggleCheckboxState.OnChange -= StateHasChanged;
        }

        private async Task LoadDataAsync(int transactionID)
        {
            IsLoading = true;

            var getStartingBalance = await _walletsService.GetStartingBalance(transactionID);
            Wallet = getStartingBalance.Result;

            Input.TransactionID = transactionID;
            LoadExistingData();

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private void LoadExistingData()
        {
            _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "checked", true);

            _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "disabled", true);

            Input.Amount = ToFixed4(Wallet.Amount);
            Input.UnitPriceInUSD = ToFixed4(Wallet.UnitPriceInUSD);
        }

        private void CalculateAmount()
        {
            Total = Input.Amount * Input.UnitPriceInUSD;

            StateHasChanged();
        }

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private async void OnSubmitInternalTransferAsync()
        {
            IsLoading = true;

            StateHasChanged();

            var result = await _walletsService.UpdateStartingBalance(Input);

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
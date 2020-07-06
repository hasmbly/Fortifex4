using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.ExternalTransfers.Commands.UpdateExternalTransfer;
using Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static Fortifex4.WebUI.Shared.Common.ToggleCheckbox;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditExternalTransfer
    {

        public string Title { get; set; } = "Edit External Transfer";

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
        public string ToggleCheckboxElementID { get; set; } = "edit-external-transfer-direction";

        public UpdateExternalTransferRequest Input { get; set; } = new UpdateExternalTransferRequest();

        public GetExternalTransferResponse Wallet { get; set; } = new GetExternalTransferResponse();

        public ToggleCheckboxAttributes Attributes { get; set; } = new ToggleCheckboxAttributes
            (new ToggleCheckboxAttributesValue
            {
                ElementID = "edit-external-transfer-direction"
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

        public IList<WalletSameCurrencyDTO> Wallets { get; set; } = new List<WalletSameCurrencyDTO>();

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

            var getWalletExternalTransfer = await _externalTransfersService.GetExternalTransfer(transactionID);
            Wallet = getWalletExternalTransfer.Result;

            Input.TransactionID = transactionID;
            LoadExistingData();

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private void LoadExistingData()
        {
            Input.Amount = ToFixed4(Wallet.Amount);
            Input.UnitPriceInUSD = ToFixed4(Wallet.UnitPriceInUSD);
            Input.TransactionDateTime = Wallet.TransactionDateTime;
            Input.PairWalletName = Wallet.PairWalletName;
            Input.PairWalletAddress = Wallet.PairWalletAddress;

            if (Wallet.TransactionType == TransactionType.ExternalTransferIN)
            {
                _toggleCheckboxState.SetToggle(ToggleCheckboxElementID, true);

                Input.TransferDirection = TransferDirection.IN;
            }
            else
            {
                _toggleCheckboxState.SetToggle(ToggleCheckboxElementID, false);

                Input.TransferDirection = TransferDirection.OUT;
            }
        }

        private void OnChangeDirection(bool state)
        {
            Console.WriteLine($"OnChangeDirection - state: {state}");

            if (state)
            {
                Input.TransferDirection = TransferDirection.IN;

                StateHasChanged();
            }
            else
            {
                Input.TransferDirection = TransferDirection.OUT;

                StateHasChanged();
            }
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

            OnChangeDirection(_toggleCheckboxState.IsChecked);

            var result = await _externalTransfersService.UpdateExternalTransfer(Input);

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
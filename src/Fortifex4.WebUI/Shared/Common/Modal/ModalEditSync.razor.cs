using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Sync.Commands.UpdateSync;
using Fortifex4.Shared.Sync.Queries.GetSync;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static Fortifex4.WebUI.Shared.Common.ToggleCheckbox;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditSync
    {
        private bool _disposed = false;

        public string Title { get; set; } = "Edit Sync";

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

        public const string ToggleCheckboxElementID = "edit-sync";

        public UpdateSyncRequest Input { get; set; } = new UpdateSyncRequest();

        public GetSyncResponse Wallet { get; set; } = new GetSyncResponse();

        public ToggleCheckboxAttributes Attributes { get; set; } = new ToggleCheckboxAttributes
            (new ToggleCheckboxAttributesValue
            {
                ElementID = ToggleCheckboxElementID
            });

        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public string PairWalletAddress { get; set; }

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _toggleCheckboxState.OnChange -= StateHasChanged;
            }

            _disposed = true;
        }

        private async Task LoadDataAsync(int transactionID)
        {
            IsLoading = true;

            var getSyncPersonalWallet = await _walletsService.GetSyncPersonalWallet(transactionID);
            Wallet = getSyncPersonalWallet.Result;

            Input.TransactionID = transactionID;
            LoadExistingData();

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private void LoadExistingData()
        {
            Input.PairWalletName = Wallet.PairWalletName;
            Input.UnitPriceInUSD = ToFixed4(Wallet.UnitPriceInUSD);

            _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "disabled", false);

            if (Wallet.TransactionType == TransactionType.SyncTransactionIN)
            {
                _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "checked", true);
            }
            else
            {
                _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "checked", false);
            }

            _toggleCheckboxState.SetToggleProp(ToggleCheckboxElementID, "disabled", true);

            Amount = ToFixed4(Wallet.Amount);
            TransactionDateTime = Wallet.TransactionDateTime;
            PairWalletAddress = Wallet.PairWalletAddress;
        }

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private void CalculateAmount()
        {
            Total = Amount * Input.UnitPriceInUSD;

            StateHasChanged();
        }

        private async void OnSubmitInternalTransferAsync()
        {
            IsLoading = true;

            StateHasChanged();

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
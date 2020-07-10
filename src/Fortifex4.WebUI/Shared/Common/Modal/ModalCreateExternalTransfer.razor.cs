using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static Fortifex4.WebUI.Shared.Common.ToggleCheckbox;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateExternalTransfer
    {

        public string Title { get; set; } = "Add External Transfer";

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
        public string LabelAmount { get; set; }
        public string ToggleCheckboxElementID { get; set; } = "create-external-transfer-direction";

        public CreateExternalTransferRequest Input { get; set; } = new CreateExternalTransferRequest();

        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();
      
        // this select option will use in Transaction Index (IsPublic -> True)
        public string SelectWallet
        {
            get => Input.WalletID.ToString();
            set
            {
                Input.WalletID = int.Parse(value);

                OnChangeSelectWallet(Input.WalletID);
            }
        }

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

        public ToggleCheckboxAttributes Attributes { get; set; } = new ToggleCheckboxAttributes
            (new ToggleCheckboxAttributesValue 
            { 
                ElementID = "create-external-transfer-direction"
            });

        protected async override Task OnInitializedAsync()
        {
            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            _toggleCheckboxState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _toggleCheckboxState.OnChange -= StateHasChanged;
        }

        private async Task LoadDataAsync()
        {
            IsLoading = true;

            _toggleCheckboxState.SetToggle(ToggleCheckboxElementID, true);

            SetDefaultValue();

            if (WalletID.HasValue)
            {
                var getWallet = await _walletsService.GetWallet(WalletID.Value);
                Wallet = getWallet.Result;

                Input.WalletID = WalletID.Value;

                await ConvertPrice(Wallet.MainPocket.CurrencySymbol);
            }
            else
            {
                IsPublic = true;

                await LoadSelectOptionWallet();
            }

            IsLoading = false;

            StateHasChanged();
        }
        
        private void SetDefaultValue()
        {
            Input.TransferDirection = TransferDirection.IN;
            Input.Amount = 0m;
            Input.UnitPriceInUSD = 0m;
            Input.TransactionDateTime = DateTime.Now;
        }

        private async Task LoadSelectOptionWallet()
        {
            var getAllWalletsWithSameCurrency = await _walletsService.GetAllWalletsWithSameCurrency(User.Identity.Name);
            Wallets = getAllWalletsWithSameCurrency.Result.Wallets.ToList();

            Input.WalletID = Wallets.First().WalletID;

            OnChangeSelectWallet(Input.WalletID);
        }

        #region EventHandler
        private void OnChangeDirection(bool state)
        {
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

        private async void OnChangeSelectWallet(int walletID)
        {
            IsLoading = true;

            Wallet.MainPocket.CurrencyName = Wallets.Where(x => x.WalletID == walletID).First().CurrencyName;

            string symbol = Wallets.Where(x => x.WalletID == walletID).First().CurrencySymbol;
            
            await ConvertPrice(symbol);

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private void CalculateAmount()
        {
            Total = Input.Amount * Input.UnitPriceInUSD;

            StateHasChanged();
        }

        private async Task ConvertPrice(string symbol)
        {
            var result = await _toolsService.GetUnitPriceInUSD(symbol);

            Input.UnitPriceInUSD = ToFixed4(result.Result.UnitPriceInUSD);

            StateHasChanged();
        }
        #endregion

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private async void OnSubmitInternalTransferAsync()
        {
            StateHasChanged();

            Console.WriteLine($"OnSubmit - IsChecked: {_toggleCheckboxState.IsChecked}");

            OnChangeDirection(_toggleCheckboxState.IsChecked);

            IsLoading = true;

            var result = await _externalTransfersService.CreateExternalTransfer(Input);

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
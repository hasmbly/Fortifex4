using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

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

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; }
        public bool IsPublic { get; set; }
        public bool IsChecked { get; set; }
        public decimal Total { get; set; }
        public string LabelAmount { get; set; }

        public CreateExternalTransferRequest Input { get; set; } = new CreateExternalTransferRequest();

        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();
      
        public DotNetObjectReference<ToggleDirection> ToggleDirection { get; set; }

        // this select option will use if IsPublic -> True
        public string SelectWallet
        {
            get => Input.WalletID.ToString();
            set
            {
                Input.WalletID = int.Parse(value);

                OnChangeSelectWallet(Input.WalletID);
            }
        }

        public bool Direction
        {
            get => IsChecked;
            set
            {
                IsChecked = value;

                Console.WriteLine($"IsChecked: {IsChecked}");

                StateHasChanged();
            }
        }

        public int Amount
        {
            get => (int)Input.Amount;
            set
            {
                Input.Amount = value;

                Console.WriteLine("OnSetAmount");

                CalculateAmount();
            }
        }

        public IList<WalletSameCurrencyDTO> Wallets { get; set; } = new List<WalletSameCurrencyDTO>();

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;

            StateHasChanged();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ToggleDirection = DotNetObjectReference.Create(new ToggleDirection(false));

                JsRuntime.InvokeVoidAsync("Toggle.init");
                JsRuntime.InvokeVoidAsync("Toggle.onChangeToggle", "#external-transfer-toggle-direction", ToggleDirection);
            }
        }

        private async Task LoadDataAsync()
        {
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

            // default value for select option -> SelectedWallet
            Input.WalletID = Wallets.First().WalletID;
        }

        #region EventHandler
        private void OnChangeDirection(bool state)
        {
            Console.WriteLine($"OnChangeDirection - state: {state}");

            if (state)
            {
                LabelAmount = "Incoming Amount";
                Input.TransferDirection = TransferDirection.IN;

                StateHasChanged();
            }
            else
            {
                LabelAmount = "Outgoing Amount";
                Input.TransferDirection = TransferDirection.OUT;

                StateHasChanged();
            }
        }

        private async void OnChangeSelectWallet(int walletID)
        {
            IsLoading = true;

            Wallet.Name = Wallets.Where(x => x.WalletID == walletID).First().Name;

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

            Input.UnitPriceInUSD = result.Result.UnitPriceInUSD;

            StateHasChanged();
        }
        #endregion

        private async void OnSubmitInternalTransferAsync()
        {
            StateHasChanged();

            IsChecked = ToggleDirection.Value.IsChecked;

            Console.WriteLine($"OnSubmit - IsChecked: {IsChecked}");

            OnChangeDirection(IsChecked);

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

    public class ToggleDirection
    {
        public bool IsChecked { get; set; }

        public ToggleDirection(bool isChecked) => IsChecked = isChecked;

        [JSInvokable]
        public void SetIsChecked(bool checkedState) 
        {
            IsChecked = checkedState;
            
            Console.WriteLine($"SetIsChecked: {IsChecked}");
        }
    }
}
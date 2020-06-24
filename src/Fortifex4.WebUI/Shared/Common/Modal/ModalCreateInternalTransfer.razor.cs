using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateInternalTransfer
    {
        public string Title { get; set; } = "Add Internal Transfer";

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

        public GetWalletResponse MyProperty { get; set; }

        public CreateInternalTransferRequest Input { get; set; } = new CreateInternalTransferRequest();
        
        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();

        // this select option will use if IsPublic -> True
        public string SelectFromWallet
        {
            get => Input.FromPocketID.ToString();
            set 
            {
                Input.FromPocketID = int.Parse(value);
                
                OnChangeFromWallet(Input.FromPocketID);
            }
        }

        public string SelectToWallet
        {
            get => Input.ToPocketID.ToString();
            set => Input.ToPocketID = int.Parse(value);
        }

        public IList<WalletSameCurrencyDTO> ListFromWallets { get; set; } = new List<WalletSameCurrencyDTO>();
        public IList<WalletSameCurrencyDTO> ListToWallets { get; set; } = new List<WalletSameCurrencyDTO>();

        public int? ExcludeToWalletID { get; set; }

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;

            StateHasChanged();
        }

        private async Task LoadDataAsync()
        {
            if (WalletID.HasValue)
            {
                // load current wallet details by id
                var getWallet = await _walletsService.GetWallet(WalletID.Value);
                Wallet = getWallet.Result;

                Input.FromPocketID = Wallet.MainPocket.PocketID;

                await LoadSelectOptionToWallet();
            }
            else
            {
                IsPublic = true;

                await LoadSelectOptionFromWallet();
            }

            Input.TransactionDateTime = DateTime.Now;

            DistinctFromAndToCurrency();
        }

        private async Task LoadSelectOptionToWallet()
        {
            var getWalletsWithSameCurrency = await _internalTransfersService.GetWalletsWithSameCurrency(WalletID.Value);
            ListToWallets = getWalletsWithSameCurrency.Result.Wallets.ToList();

            // default value for select option -> SelectedToWallet
            Input.ToPocketID = ListToWallets.Count > 0 ? ListToWallets.First().PocketID : 0;
        }

        private async Task LoadSelectOptionFromWallet()
        {
            var getAllWalletsWithSameCurrency = await _internalTransfersService.GetAllWalletsWithSameCurrency(User.Identity.Name);
            ListFromWallets = getAllWalletsWithSameCurrency.Result.Wallets.ToList();

            // default value for select option -> SelectedToWallet
            Input.FromPocketID = ListFromWallets.First().PocketID;
        }

        #region EventHandler
        private async void OnChangeFromWallet(int pocketID)
        {
            IsLoading = true;

            SetCurrencyName(pocketID);

            int walletID = ListFromWallets.Where(x => x.PocketID == pocketID).First().WalletID;
            await LoadSelectOptionToWallet();

            DistinctFromAndToCurrency();

            IsLoading = false;

            StateHasChanged();
        }

        private void SetCurrencyName(int pocketID)
        {
            string currencyName = ListFromWallets.Where(x => x.PocketID == pocketID).First().CurrencyName;
            Wallet.MainPocket.CurrencyName = currencyName;
        }

        private void DistinctFromAndToCurrency()
        {
            if (Input.FromPocketID == Input.ToPocketID)
            {
                ExcludeToWalletID = Input.FromPocketID;
            }

            StateHasChanged();
        }
        #endregion

        private async void OnSubmitInternalTransferAsync()
        {
            StateHasChanged();

            IsLoading = true;

            var result = await _internalTransfersService.CreateInternalTransfer(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo($"/wallets/details/{result.Result.WalletID}");

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
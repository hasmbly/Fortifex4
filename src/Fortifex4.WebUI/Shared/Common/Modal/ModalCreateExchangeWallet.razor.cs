using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies;
using Fortifex4.Shared.Wallets.Commands.CreateExchangeWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateExchangeWallet
    {
        public string Title { get; set; } = "Add Exchange Wallet";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int OwnerID { get; set; }

        public bool IsLoading { get; set; } = false;
        
        public string OwnerProviderName { get; set; }

        public BaseModal BaseModal { get; set; }

        public ClaimsPrincipal User { get; set; }


        public CreateExchangeWalletRequest Input { get; set; } = new CreateExchangeWalletRequest();

        public IList<CurrencyDTO> Currencies { get; set; } = new List<CurrencyDTO>();

        public string SelectedCurrency
        {
            get => Input.CurrencyID.ToString();
            set => Input.CurrencyID = int.Parse(value);
        }

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
            Input.OwnerID = OwnerID;

            var getOwner = await _ownersService.GetOwner(OwnerID);
            OwnerProviderName = getOwner.Result.ProviderName;

            var getAvailableCurrencies = await _currenciesService.GetAvailableCurrencies(OwnerID);
            Currencies = getAvailableCurrencies.Result.Currencies.ToList();

            Input.CurrencyID = Currencies.First().CurrencyID;
        }

        private async void OnSubmitPersonalWalletAsync()
        {
            StateHasChanged();

            IsLoading = true;

            var result = await _walletsService.CreateExchangeWallet(Input);

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
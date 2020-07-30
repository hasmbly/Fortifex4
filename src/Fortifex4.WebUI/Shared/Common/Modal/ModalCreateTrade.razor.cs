using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember;
using Fortifex4.Shared.Lookup.Queries.GetOwners;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.Shared.Trades.Commands.CreateTrade;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using static Fortifex4.WebUI.Shared.Common.ToggleCheckbox;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateTrade
    {
        private bool _disposed = false;

        public string Title { get; set; } = "Add Trade";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int? OwnerID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; }
        public bool IsPublic { get; set; }

        public string LabelAmount { get; set; } = "Amount Bought";
        public string LabelUnitPrice { get; set; } = "Unit Buy Price";

        public decimal Total { get; set; }
        public decimal TotalInUSD { get; set; }

        public const string ToggleCheckboxTradeTypeElementID = "create-trade-type";
        public const string ToggleCheckboxWithHoldElementID = "create-trade-withhold";

        public CreateTradeRequest Input { get; set; } = new CreateTradeRequest();

        public GetOwnerResponse Owner { get; set; } = new GetOwnerResponse();
        public GetOwnersResponse Owners { get; set; } = new GetOwnersResponse();

        public ToggleCheckboxAttributes TradeTypeAttributes { get; set; } = new ToggleCheckboxAttributes(new ToggleCheckboxAttributesValue
        {
            ElementID = ToggleCheckboxTradeTypeElementID,
            DataOn = "Buy",
            DataOff = "Sell",
            DataOnStyle = "success",
            DataOffStyle = "danger"
        });

        public ToggleCheckboxAttributes WithHoldAttributes { get; set; } = new ToggleCheckboxAttributes(new ToggleCheckboxAttributesValue
        {
            ElementID = ToggleCheckboxWithHoldElementID,
            DataOn = "Yes",
            DataOff = "No",
            DataOnStyle = "success",
            DataOffStyle = "danger"
        });

        public string SelectFromCurrencies
        {
            get => Input.FromCurrencyID.ToString();
            set
            {
                Input.FromCurrencyID = int.Parse(value);

                _ = GetUnitPriceInUSD();
                _ = GetUnitPrice();
            }
        }

        public string SelectPairCurrencies
        {
            get => Input.ToCurrencyID.ToString();
            set
            {
                Input.ToCurrencyID = int.Parse(value);

                // check CurrencyType to change toggle property
                CheckPairCurrencyType(int.Parse(value));

                _ = GetUnitPrice();
            }
        }

        public string SelectedExchangeProvider
        {
            get => Input.OwnerID.ToString();
            set
            {
                Input.OwnerID = int.Parse(value);
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

        public decimal UnitPrice
        {
            get => ToFixed4(Input.UnitPrice);
            set
            {
                Input.UnitPrice = ToFixed4(value);

                CalculateAmount();
            }
        }

        public decimal UnitPriceInUSD
        {
            get => ToFixed4(Input.UnitPriceInUSD);
            set
            {
                Input.UnitPriceInUSD = ToFixed4(value);

                CalculateAmount();
            }
        }

        public IList<OwnerDTO> Exchanges { get; set; } = new List<OwnerDTO>();
        public IList<CoinCurrencyDTO> FromCurrencies { get; set; } = new List<CoinCurrencyDTO>();
        public IList<CurrencyDTO> PairCurrencies { get; set; } = new List<CurrencyDTO>();

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

        private async Task LoadDataAsync()
        {
            IsLoading = true;

            SetDefaultToggleCheckbox();

            SetDefaultValue();

            if (OwnerID.HasValue)
            {
                var getOwner = await _ownersService.GetOwner(OwnerID.Value);
                Owner = getOwner.Result;

                Input.OwnerID = OwnerID.Value;
            }
            else
            {
                IsPublic = true;

                await LoadSelectOwnersExchange();
            }

            await LoadSelectFromCurrencies();
            await LoadSelectPairCurrency();

            await GetUnitPriceInUSD();
            await GetUnitPrice();

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private void SetDefaultToggleCheckbox()
        {
            _toggleCheckboxState.SetToggle(ToggleCheckboxTradeTypeElementID, true);
            _toggleCheckboxState.SetToggle(ToggleCheckboxWithHoldElementID, false);
        }

        private void SetDefaultValue()
        {
            Input.TradeType = TradeType.Buy;
            Input.Amount = 1m;
            Input.UnitPrice = 0m;
            Input.UnitPriceInUSD = 0m;
            Input.TransactionDateTime = DateTime.Now;
        }

        private async Task LoadSelectOwnersExchange()
        {
            var getOwners = await _ownersService.GetOwners(User.Identity.Name);
            Exchanges = getOwners.Result.Owners.ToList();

            Input.OwnerID = Exchanges.First().OwnerID;
        }

        private async Task LoadSelectFromCurrencies()
        {
            var getAllCoinCurrencies = await _currenciesService.GetAllCoinCurrencies();
            FromCurrencies = getAllCoinCurrencies.Result.CoinCurrencies.ToList();

            await SetFromCurencyID();
        }

        private async Task LoadSelectPairCurrency()
        {
            var getDestinationCurrenciesForMember = await _currenciesService.GetDestinationCurrenciesForMember(User.Identity.Name);
            PairCurrencies = getDestinationCurrenciesForMember.Result.Currencies.ToList();

            Input.ToCurrencyID = PairCurrencies.First().CurrencyID;

            CheckPairCurrencyType(Input.ToCurrencyID);
        }

        private async Task SetFromCurencyID()
        {
            var getPreferences = await _membersService.GetPreferences(User.Identity.Name);
            Input.FromCurrencyID = getPreferences.Result.PreferredCoinCurrencyID;
        }

        private void CheckPairCurrencyType(int currencyID)
        {
            CurrencyType currencyType = PairCurrencies.Where(x => x.CurrencyID == currencyID).First().CurrencyType;

            // change property toggle with Javascript;
            if (currencyType == CurrencyType.Fiat)
            {
                _toggleCheckboxState.SetToggleProp(ToggleCheckboxWithHoldElementID, "checked", false);

                _toggleCheckboxState.SetToggleProp(ToggleCheckboxWithHoldElementID, "disabled", true);
            }
            else
            {
                _toggleCheckboxState.SetToggleProp(ToggleCheckboxWithHoldElementID, "disabled", false);
            }
        }

        #region Event Callback Handler
        private void OnChangeCheckedToggleState(string elementID)
        {
            if (elementID == $"#{ToggleCheckboxTradeTypeElementID}")
            {
                OnChangeTradeType(_toggleCheckboxState.IsChecked);
            }
            else if (elementID == $"#{ToggleCheckboxWithHoldElementID}")
            {
                OnChangeWithHold(_toggleCheckboxState.IsChecked);
            }
        }

        private void OnChangeTradeType(bool state)
        {
            if (state)
            {
                Input.TradeType = TradeType.Buy;

                LabelAmount = "Amount Bought";
                LabelUnitPrice = "Unit Buy Price";

                StateHasChanged();
            }
            else
            {
                Input.TradeType = TradeType.Sell;

                LabelAmount = "Amount Sold";
                LabelUnitPrice = "Unit Sell Price";

                StateHasChanged();
            }
        }

        private void OnChangeWithHold(bool state)
        {
            if (state)
            {
                Input.IsWithholding = true;

                StateHasChanged();
            }
            else
            {
                Input.IsWithholding = false;

                StateHasChanged();
            }
        }
        #endregion

        private async Task GetUnitPriceInUSD()
        {
            string symbol = GetFromCurrencySymbol(Input.FromCurrencyID);

            var result = await _toolsService.GetUnitPriceInUSD(symbol);

            Input.UnitPriceInUSD = ToFixed4(result.Result.UnitPriceInUSD);

            StateHasChanged();

            CalculateAmount();
        }

        private async Task GetUnitPrice()
        {
            string fromCurrencySymbol = GetFromCurrencySymbol(Input.FromCurrencyID);
            string toCurrencySymbol = GetPairCurrencySymbol(Input.ToCurrencyID);

            var result = await _toolsService.GetUnitPrice(fromCurrencySymbol, toCurrencySymbol);

            Input.UnitPrice = ToFixed4(result.Result.UnitPrice);

            StateHasChanged();

            CalculateAmount();
        }

        private void CalculateAmount()
        {
            Total = Input.Amount * Input.UnitPrice;
            TotalInUSD = Input.Amount * Input.UnitPriceInUSD;

            StateHasChanged();
        }

        private decimal ToFixed4(decimal value)
        {
            value = decimal.Parse(value.ToString("N4").Replace(".0000", ""));

            return value;
        }

        private string GetFromCurrencySymbol(int currencyID)
        {
            return FromCurrencies.Where(x => x.CurrencyID == currencyID).First().Symbol;
        }

        private string GetPairCurrencySymbol(int currencyID)
        {
            return PairCurrencies.Where(x => x.CurrencyID == currencyID).First().Symbol;
        }

        private async void OnSubmitAsync()
        {
            StateHasChanged();

            OnChangeWithHold(_toggleCheckboxState.IsChecked);
            
            IsLoading = true;

            var result = await _tradesService.CreateTrade(Input);

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
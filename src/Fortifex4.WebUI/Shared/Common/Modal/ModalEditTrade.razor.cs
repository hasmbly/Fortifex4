using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Trades.Commands.UpdateTrade;
using Fortifex4.Shared.Trades.Queries.GetTrade;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditTrade
    {
        public string Title { get; set; } = "Edit Trade";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; }

        public string LabelAmount { get; set; } = "Amount Bought";
        public string LabelUnitPrice { get; set; } = "Unit Buy Price";

        public string OwnerProviderName { get; set; }
        public string SourceCurrencySymbol { get; set; }
        public string DestinationCurrencySymbol { get; set; }
        public string Withholding { get; set; }
        public string TypeOfTrade { get; set; }

        public decimal Total { get; set; }
        public decimal TotalInUSD { get; set; }

        public UpdateTradeRequest Input { get; set; } = new UpdateTradeRequest();

        public GetTradeResponse Trade { get; set; } = new GetTradeResponse();

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
            get => Input.UnitPrice;
            set
            {
                Input.UnitPrice = ToFixed4(value);

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
        }

        private async Task LoadDataAsync(int tradeID)
        {
            IsLoading = true;

            var getTrade = await _tradesService.GetTrade(tradeID);
            Trade = getTrade.Result;

            Input.TradeID = tradeID;
            await LoadExistingData();

            CalculateAmount();

            IsLoading = false;

            StateHasChanged();
        }

        private async Task LoadExistingData()
        {
            if (Trade.TradeType == TradeType.Buy)
            {
                TypeOfTrade = "Buy";
                LabelAmount = "Amount Bought";
                LabelUnitPrice = "Unit Buy Price";
            }
            else
            {
                TypeOfTrade = "Sell";
                LabelAmount = "Amount Sold";
                LabelUnitPrice = "Unit Sell Price";
            }

            if (Trade.IsWithholding)
                Withholding = "Yes";
            else
                Withholding = "No";

            OwnerProviderName = await GetOwnerProviderName(Trade.OwnerID);
            SourceCurrencySymbol = await GetSourceCurrencySymbol(Trade.SourceCurrencyID);
            DestinationCurrencySymbol = await GetDestinationCurrencySymbol(Trade.DestinationCurrencyID);

            Input.TradeType = Trade.TradeType;
            Input.Amount = ToFixed4(Trade.AbsoluteAmount);
            Input.TransactionDateTime = Trade.TransactionDateTime;
            Input.UnitPrice = ToFixed4(Trade.UnitPrice);
            Input.UnitPriceInUSD = ToFixed4(Trade.UnitPriceInUSD);
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

        private async Task<string> GetSourceCurrencySymbol(int fromCurrencyID)
        {
            var result = await _currenciesService.GetCurrency(fromCurrencyID);

            return result.Result.Symbol;
        }

        private async Task<string> GetDestinationCurrencySymbol(int pairCurrencyID)
        {
            var result = await _currenciesService.GetCurrency(pairCurrencyID);

            return result.Result.Symbol;
        }

        private async Task<string> GetOwnerProviderName(int ownerID)
        {
            var result = await _ownersService.GetOwner(ownerID);

            return result.Result.ProviderName;
        }

        private async void OnSubmitAsync()
        {
            StateHasChanged();

            IsLoading = true;

            var result = await _tradesService.UpdateTrade(Input);

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
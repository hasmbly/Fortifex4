using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using CurrencyDTO = Fortifex4.Shared.Currencies.Queries.GetDistinctCurrenciesByMemberID.CurrencyDTO;

namespace Fortifex4.WebUI.Pages
{
    public partial class PieCharts
    {
        private bool _disposed = false;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Inject]
        public IConfiguration _configuration { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool IsLoading { get; set; }

        public GetPortfolioResponse Portfolio { get; set; } = new GetPortfolioResponse();

        public string SelectDropdownOptions
        {
            get 
            {
                return SelectDropdownOptionsValue;
            }
            set
            {
                SelectDropdownOptionsValue = value;

                OnChangeSelectDropdownOptions(value);
            } 
        }

        public string SelectCryptoCurrency
        {
            get 
            { 
                return SelectCryptoCurrencyValue;
            }
            set 
            {
                SelectCryptoCurrencyValue = value;

                OnChangeSelectCryptoCurrency(value);
            }
        }

        public string SelectDropdownOptionsValue { get; set; }
        public string SelectCryptoCurrencyValue { get; set; }

        public IList<SelectListItem> DropdownOptions { get; set; }

        public IList<CurrencyDTO> CryptoCurrency { get; set; }

        protected async override Task OnInitializedAsync()
        {
            globalState.ShouldRender += RefreshMe;

            await InitAsync();
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
                globalState.ShouldRender -= RefreshMe;
            }

            _disposed = true;
        }

        private async void RefreshMe() => await InitAsync();

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            DropdownOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Portfolio By Coin" },
                new SelectListItem { Value = "2", Text = "Portfolio By Exchange" },
                new SelectListItem { Value = "3", Text = "Coin By Exchange" }
            };

            SelectDropdownOptions = DropdownOptions.First().Value;

            Portfolio = Task.FromResult(await _portfolioService.GetPortfolio(User.Identity.Name)).Result.Result;

            IsLoading = false;

            StateHasChanged();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetPortfolioByCoin();
            }
        }

        #region Event Handler
        private async void OnChangeSelectDropdownOptions(string value)
        {
            await DestroyPieChart();

            switch (value)
            {
                case "1": await GetPortfolioByCoin(); break;
                case "2": await GetPortfolioByExchanges(); break;
                case "3": await SetCryptoDropdown(); break;
                default: break;
            }
        }

        private async void OnChangeSelectCryptoCurrency(string currencyID)
        {
            await DestroyPieChart();

            string selectedCoinName = CryptoCurrency.Where(x => x.CurrencyID == int.Parse(currencyID)).First().Name;

            await GetCoinByExchange(int.Parse(currencyID), selectedCoinName);
        }
        #endregion

        private async Task GetPortfolioByCoin()
        {
            IsLoading = true;

            var result = Task.FromResult(await _chartsService.GetPortfolioByCoinsV2(User.Identity.Name)).Result.Result;

            var config = new PieChartsConfig() { Title = "Porfolio By Coin" };

            if (result.IsSuccessful)
            {
                foreach (var currency in result.Currencies)
                {
                    config.Labels.Add(currency.Name);
                    config.Data.Add(currency.CurrentValueInPreferredFiatCurrency);
                }

                IsLoading = false;

                StateHasChanged();

                await InitPieChart(config);
            }
        }

        private async Task GetPortfolioByExchanges()
        {
            IsLoading = true;

            var result = Task.FromResult(await _chartsService.GetPortfolioByExchanges(User.Identity.Name)).Result.Result;

            var config = new PieChartsConfig() { Title = "Porfolio By Exchange" };

            if (result.IsSuccessful)
            {
                config.Labels = result.Labels;
                config.Data = result.Value;

                IsLoading = false;

                StateHasChanged();

                await InitPieChart(config);
            }
        }

        private async Task GetCoinByExchange(int currencyID, string SelectedCoinName)
        {
            IsLoading = true;

            var result = Task.FromResult(await _chartsService.GetCoinByExchanges(User.Identity.Name, currencyID)).Result.Result;

            var config = new PieChartsConfig() { Title = $"Porfolio Coin By Exchange In {SelectedCoinName}" };

            if (result.IsSuccessful)
            {
                config.Labels = result.Labels;
                config.Data = result.Value;

                IsLoading = false;

                StateHasChanged();

                await InitPieChart(config);
            }
        }

        private async Task SetCryptoDropdown()
        {
            IsLoading = true;

            var result = Task.FromResult(await _currenciesService.GetDistinctCurrenciesByMemberID(User.Identity.Name)).Result.Result;

            CryptoCurrency = result.Currencies;

            SelectCryptoCurrency = CryptoCurrency.First().CurrencyID.ToString();

            IsLoading = false;

            StateHasChanged();
        }

        #region PieChart
        private async Task InitPieChart(PieChartsConfig config) => await JsRuntime.InvokeVoidAsync("PieChart.init", config);

        private async Task DestroyPieChart() => await JsRuntime.InvokeVoidAsync("PieChart.destroy");
        #endregion

        public class SelectListItem
        {
            public string Value { get; set; }
            public string Text { get; set; }
        }

        public class PieChartsConfig
        {
            public string PieChartElementID { get; set; }
            public List<string> BackgroundColor { get; set; }

            public string Title { get; set; }
            public IList<decimal?> Data { get; set; }
            public IList<string> Labels { get; set; }

            public PieChartsConfig()
            {
                PieChartElementID = "myChart";

                BackgroundColor = new List<string>()
                {
                    "rgba(255, 99, 132, 1)",
                    "rgba(54, 162, 235, 1)",
                    "rgba(255, 206, 86, 1)",
                    "rgba(75, 192, 192, 1)",
                    "rgba(153, 102, 255, 1)",
                    "rgba(255, 159, 64, 1)"
                };

                Data = new List<decimal?>();
                Labels = new List<string>();
            }
        }
    }
}
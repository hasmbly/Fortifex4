﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages
{
    public partial class Portfolio
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        public bool IsLoading { get; set; }
        public bool FirstStage { get; set; }

        public GetPortfolioResponse _Portfolio { get; set; } = new GetPortfolioResponse();

        public IList<CurrencyDTO> ValidCurrencies { get; set; } = new List<CurrencyDTO>();
        public IList<CurrencyDTO> TopCurrencies { get; set; } = new List<CurrencyDTO>();
        public IList<CurrencyDTO> BottomCurrencies { get; set; } = new List<CurrencyDTO>();

        protected async override Task OnInitializedAsync()
        {
            globalState.ShouldRender += RefreshMe;

            await InitAsync();
        }

        public void Dispose()
        {
            globalState.ShouldRender -= RefreshMe;
        }

        private async void RefreshMe()
        {
            System.Console.WriteLine($"Portfolio was Reloaded");

            await InitAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            System.Console.WriteLine($"--OnAfterRender--");

            if (firstRender)
            {
                System.Console.WriteLine($"OnAfterRender - This is First Render");
            }
            else if (FirstStage)
            {
                System.Console.WriteLine($"OnAfterRender - First Stage Done");

                // Finally, after so long.. the datables plugin from js can only be initialize after DOM element (with related id) was Ready
                await JsRuntime.InvokeVoidAsync("Portfolio.init");

                FirstStage = false;
             
                StateHasChanged();
            }
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _portfolioService.GetPortfolio(User.Identity.Name);

            _Portfolio = result.Result;

            ValidCurrencies = _Portfolio.Currencies
               .Where(x => x.CurrentValueInUSD >= Limit.MinimumAssetValueInUSD)
               .OrderByDescending(x => x.CurrentValueInPreferredFiatCurrency)
               .ToList();

            IsLoading = false;

            StateHasChanged();

            FirstStage = true;
        }
    }
}
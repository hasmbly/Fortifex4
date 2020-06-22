using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.Shared.Members.Commands.UpdatePreferredCoinCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame;
using Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class TopBar
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public bool IsLoading { get; set; }

        public ClaimsPrincipal User { get; set; }

        public MyPreferences Model { get; set; } = new MyPreferences();

        public UpdatePreferredTimeFrameRequest UpdatePreferredTimeFrameRequest { get; set; } = new UpdatePreferredTimeFrameRequest();
        public UpdatePreferredCoinCurrencyRequest UpdatePreferredCoinCurrencyRequest { get; set; } = new UpdatePreferredCoinCurrencyRequest();
        public UpdatePreferredFiatCurrencyRequest UpdatePreferredFiatCurrencyRequest { get; set; } = new UpdatePreferredFiatCurrencyRequest();

        public string SelectedTimeFrame
        {
            get
            {
                return Model.SelectedTimeFrameValue;
            }
            set
            {
                Model.SelectedTimeFrameValue = value;
                
                OnChangeTimeFrame();
            }
        }

        public string SelectedCoinCurrency
        {
            get
            {
                return Model.SelectedCoinCurrencyValue;
            }
            set
            {
                Model.SelectedCoinCurrencyValue = value;
                
                OnChangeCoinCurrency();
            }
        }

        public string SelectedFiatCurrency
        {
            get
            {
                return Model.SelectedFiatCurrencyValue;
            }
            set
            {
                Model.SelectedFiatCurrencyValue = value;
                
                OnChangeFiatCurrency();
            }
        }

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            UpdatePreferredTimeFrameRequest.MemberUsername = User.Identity.Name;
            UpdatePreferredCoinCurrencyRequest.MemberUsername = User.Identity.Name;
            UpdatePreferredFiatCurrencyRequest.MemberUsername = User.Identity.Name;

            await LoadDataAsync();

            await InitializeDefaultValuesAsync();
        }

        private async Task LoadDataAsync()
        {
            var getAllTimeFramesResult = await _timeFramesService.GetAllTimeFrames();
            Model.TimeFrames = getAllTimeFramesResult.Result.TimeFrames.ToList();

            var getAllFiatCurrenciesResult = await _currenciesService.GetAllFiatCurrencies();
            Model.FiatCurrency = getAllFiatCurrenciesResult.Result.FiatCurrencies.ToList();

            var getPreferableCoinCurrenciesResult = await _currenciesService.GetPreferableCoinCurrencies();
            Model.CoinCurrency = getPreferableCoinCurrenciesResult.Result.CoinCurrencies.ToList();
        }

        private async Task InitializeDefaultValuesAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var preferences = await _membersService.GetPreferences(User.Identity.Name);

                Model.SelectedTimeFrameValue = preferences.Result.PreferredTimeFrameID.ToString();
                Model.SelectedCoinCurrencyValue = preferences.Result.PreferredCoinCurrencyID.ToString();
                Model.SelectedFiatCurrencyValue = preferences.Result.PreferredFiatCurrencyID.ToString();

                IsLoading = false;
            }
        }

        #region Event OnChange
        private async void OnChangeTimeFrame()
        {
            IsLoading = true;

            UpdatePreferredTimeFrameRequest.PreferredTimeFrameID = int.Parse(SelectedTimeFrame);

            var result = await _membersService.UpdatePreferredTimeFrame(UpdatePreferredTimeFrameRequest);

            if (result.Result.IsSuccessful)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async void OnChangeCoinCurrency()
        {
            IsLoading = true;

            UpdatePreferredCoinCurrencyRequest.PreferredCoinCurrencyID = int.Parse(SelectedCoinCurrency);

            var result = await _membersService.UpdatePreferredCoinCurrency(UpdatePreferredCoinCurrencyRequest);

            if (result.Result.IsSuccessful)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        private async void OnChangeFiatCurrency()
        {
            IsLoading = true;

            UpdatePreferredFiatCurrencyRequest.PreferredFiatCurrencyID = int.Parse(SelectedFiatCurrency);

            var result = await _membersService.UpdatePreferredFiatCurrency(UpdatePreferredFiatCurrencyRequest);

            if (result.Result.IsSuccessful)
            {
                IsLoading = false;
                StateHasChanged();
            }
        }
        #endregion

        #region LogoutAsync
        private async Task LogoutAsync()
        {
            IsLoading = true;

            await _authenticationService.Logout();

            _navigationManager.NavigateTo("/");
        }
        #endregion

        #region LogUsername
        private void LogUsername()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                foreach (var claim in claimsIdentity.Claims)
                {
                    Console.WriteLine(claim.Type + ":" + claim.Value);
                }
            }
            else
            {
                Console.WriteLine("The user is NOT authenticated.");
            }
        }
        #endregion
    }

    public class MyPreferences
    {
        public string SelectedTimeFrameValue { get; set; }
        public string SelectedCoinCurrencyValue { get; set; }
        public string SelectedFiatCurrencyValue { get; set; }

        public IList<TimeFrameDTO> TimeFrames { get; set; }
        public IList<FiatCurrencyDTO> FiatCurrency { get; set; }
        public IList<CoinCurrencyDTO> CoinCurrency { get; set; }

        public MyPreferences()
        {
            TimeFrames = new List<TimeFrameDTO>();
            FiatCurrency = new List<FiatCurrencyDTO>();
            CoinCurrency = new List<CoinCurrencyDTO>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using Fortifex4.Shared.Members.Commands.UpdatePreferredCoinCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency;
using Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame;
using Fortifex4.Shared.Members.Queries.GetPreferences;
using Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class TopBar
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

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
            _httpClient = ((ServerAuthenticationStateProvider)_authenticationStateProvider).Client();

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            UpdatePreferredTimeFrameRequest.MemberUsername = User.Identity.Name;
            UpdatePreferredCoinCurrencyRequest.MemberUsername = User.Identity.Name;
            UpdatePreferredFiatCurrencyRequest.MemberUsername = User.Identity.Name;

            await LoadDataAsync();

            await InitializeDefaultValuesAsync();
        }

        #region LoadDataAsync
        private async Task LoadDataAsync()
        {
            var getAllTimeFramesResult = await _httpClient.GetJsonAsync<ApiResponse<GetAllTimeFramesResponse>>(Constants.URI.TimeFrames.GetAllTimeFrames);
            Model.TimeFrames = getAllTimeFramesResult.Result.TimeFrames.ToList();

            var getAllFiatCurrenciesResult = await _httpClient.GetJsonAsync<ApiResponse<GetAllFiatCurrenciesResponse>>(Constants.URI.Currencies.GetAllFiatCurrencies);
            Model.FiatCurrency = getAllFiatCurrenciesResult.Result.FiatCurrencies.ToList();

            var getPreferableCoinCurrenciesResult = await _httpClient.GetJsonAsync<ApiResponse<GetPreferableCoinCurrenciesResponse>>(Constants.URI.Currencies.GetPreferableCoinCurrencies);
            Model.CoinCurrency = getPreferableCoinCurrenciesResult.Result.CoinCurrencies.ToList();
        }
        #endregion

        #region InitializeDefaultValuesAsync
        private async Task InitializeDefaultValuesAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var preferences = await _httpClient.GetJsonAsync<ApiResponse<GetPreferencesResponse>>($"{Constants.URI.Members.GetPreferences}/{User.Identity.Name}");

                Model.SelectedTimeFrameValue = preferences.Result.PreferredTimeFrameID.ToString();
                Model.SelectedCoinCurrencyValue = preferences.Result.PreferredCoinCurrencyID.ToString();
                Model.SelectedFiatCurrencyValue = preferences.Result.PreferredFiatCurrencyID.ToString();
            }
        }
        #endregion

        #region Event OnChange
        private async void OnChangeTimeFrame()
        {
            UpdatePreferredTimeFrameRequest.PreferredTimeFrameID = int.Parse(SelectedTimeFrame);

            await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredTimeFrameResponse>>(Constants.URI.Members.UpdatePreferredTimeFrame, UpdatePreferredTimeFrameRequest);
         
            await OnInitializedAsync();
        }

        private async void OnChangeCoinCurrency()
        {
            UpdatePreferredCoinCurrencyRequest.PreferredCoinCurrencyID = int.Parse(SelectedCoinCurrency);

            await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredCoinCurrencyResponse>>(Constants.URI.Members.UpdatePreferredCoinCurrency, UpdatePreferredCoinCurrencyRequest);

            await OnInitializedAsync();
        }

        private async void OnChangeFiatCurrency()
        {
            UpdatePreferredFiatCurrencyRequest.PreferredFiatCurrencyID = int.Parse(SelectedFiatCurrency);

            await _httpClient.PutJsonAsync<ApiResponse<UpdatePreferredFiatCurrencyResponse>>(Constants.URI.Members.UpdatePreferredFiatCurrency, UpdatePreferredFiatCurrencyRequest);

            await OnInitializedAsync();
        }
        #endregion

        #region LogoutAsync
        private async Task LogoutAsync()
        {
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
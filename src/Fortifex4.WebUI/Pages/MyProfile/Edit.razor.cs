using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Countries.Queries.GetAllCountries;
using Fortifex4.Shared.Genders.Queries.GetAllGenders;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.Shared.Regions.Queries.GetRegions;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages.MyProfile
{
    public partial class Edit
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        public GetMemberResponse Member { get; set; } = new GetMemberResponse();
        public UpdateMemberRequest Input { get; set; } = new UpdateMemberRequest();

        public string SelectedGender
        {
            get => Input.GenderID.ToString();
            set => Input.GenderID = int.Parse(value);
        }

        public string SelectedCountryCode
        {
            get => Input.CountryCode;
            set
            {
                Input.CountryCode = value;
                OnChangeCountryCode();
            }
        }

        public string SelectedRegion
        {
            get => Input.RegionID.ToString();
            set => Input.RegionID = int.Parse(value);
        }

        public IList<GenderDTO> Genders { get; set; } = new List<GenderDTO>();
        public IList<CountryDTO> Countries { get; set; } = new List<CountryDTO>();
        public IList<RegionDTO> Regions { get; set; } = new List<RegionDTO>();

        protected async override Task OnInitializedAsync()
        {
            _httpClient = ((ServerAuthenticationStateProvider)_authenticationStateProvider).Client();

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var result = await _httpClient.GetJsonAsync<ApiResponse<GetMemberResponse>>($"{Constants.URI.Account.GetMember}/{User.Identity.Name}");
            Member = result.Result;

            LoadDataInputMember();

            var getAllGendersResult = await _httpClient.GetJsonAsync<ApiResponse<GetAllGendersResponse>>(Constants.URI.Genders.GetAllGenders);
            Genders = getAllGendersResult.Result.Genders.ToList();

            var getAllCountriesResult = await _httpClient.GetJsonAsync<ApiResponse<GetAllCountriesResponse>>(Constants.URI.Countries.GetAllCountries);
            Countries = getAllCountriesResult.Result.Countries.ToList();

            if (!string.IsNullOrEmpty(Input.CountryCode))
            {
                var getRegionsResult = await _httpClient.GetJsonAsync<ApiResponse<GetRegionsResponse>>($"{Constants.URI.Regions.GetRegions}/{Input.CountryCode}");
                Regions = getRegionsResult.Result.Regions.ToList();
            }
        }

        private void LoadDataInputMember()
        {
            Input.MemberUsername = Member.MemberUsername;
            Input.FirstName = Member.FirstName;
            Input.LastName = Member.LastName;
            Input.GenderID = Member.GenderID;
            Input.BirthDate = Member.BirthDate;
            Input.CountryCode = Member.CountryCode;
            Input.RegionID = Member.RegionID;
        }

        private async void OnChangeCountryCode()
        {
            var getRegionsResult = await _httpClient.GetJsonAsync<ApiResponse<GetRegionsResponse>>($"{Constants.URI.Regions.GetRegions}/{SelectedCountryCode}");
            Regions = getRegionsResult.Result.Regions.ToList();

            StateHasChanged();
        }

        private async void EditMyProfileAsync()
        {
            var result = await _httpClient.PutJsonAsync<ApiResponse<UpdateMemberResponse>>(Constants.URI.Members.UpdateMember, Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo("/myprofile");
                }
                else
                {
                    System.Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
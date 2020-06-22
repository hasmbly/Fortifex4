using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Countries.Queries.GetAllCountries;
using Fortifex4.Shared.Genders.Queries.GetAllGenders;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using Fortifex4.Shared.Members.Queries.GetMember;
using Fortifex4.Shared.Regions.Queries.GetRegions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditMyProfile
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public ClaimsPrincipal User { get; set; }

        public BaseModal BaseModal { get; set; }

        public GetMemberResponse Member { get; set; } = new GetMemberResponse();

        public UpdateMemberRequest Input { get; set; } = new UpdateMemberRequest();

        public string Title { get; set; } = "Edit Profile";

        public bool IsLoading { get; set; }

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
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;
        }

        // an event occur after user choose country, then load all region base on country
        private async void OnChangeCountryCode()
        {
            var getRegionsResult = await _regionsService.GetRegions(SelectedCountryCode);
            Regions = getRegionsResult.Result.Regions.ToList();

            StateHasChanged();
        }

        private async Task LoadDataAsync()
        {
            var result = await _membersService.GetMember(User.Identity.Name);
            Member = result.Result;

            LoadExistingDataMember();

            LoadSelectListData();
        }

        private void LoadExistingDataMember()
        {
            Input.MemberUsername = Member.MemberUsername;
            Input.FirstName = Member.FirstName;
            Input.LastName = Member.LastName;
            Input.GenderID = Member.GenderID;
            Input.BirthDate = Member.BirthDate;
            Input.CountryCode = Member.CountryCode;
            Input.RegionID = Member.RegionID;
        }

        private async void LoadSelectListData()
        {
            var getAllGendersResult = await _gendersService.GetAllGenders();
            Genders = getAllGendersResult.Result.Genders.ToList();

            var getAllCountriesResult = await _countriesService.GetAllCountries();
            Countries = getAllCountriesResult.Result.Countries.ToList();

            if (!string.IsNullOrEmpty(Input.CountryCode))
            {
                var getRegionsResult = await _regionsService.GetRegions(Input.CountryCode);
                Regions = getRegionsResult.Result.Regions.ToList();
            }
        }

        private async void OnSubmitEditMyProfileAsync()
        {
            IsLoading = true;

            var result = await _membersService.UpdateMember(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
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
                    System.Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
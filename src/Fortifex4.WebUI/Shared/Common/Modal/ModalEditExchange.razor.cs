using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Owners.Commands.UpdateExchangeOwner;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditExchange
    {
        public string Title { get; set; } = "Edit Exchange";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public int OwnerID { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public BaseModal BaseModal { get; set; }

        public ClaimsPrincipal User { get; set; }

        public bool IsLoading { get; set; } = false;

        public string SiteUrl { get; set; }

        public UpdateExchangeOwnerRequest Input { get; set; } = new UpdateExchangeOwnerRequest();

        public GetOwnerResponse Owner { get; set; } = new GetOwnerResponse();

        public string SelectedExchangeProvider
        {
            get => Input.ProviderID.ToString();
            set
            {
                Input.ProviderID = int.Parse(value);

                GetSiteURL(int.Parse(value));
            }
        }

        public ExchangeProviderDTO CurrentExchangeProvider { get; set; } = new ExchangeProviderDTO();

        public List<ExchangeProviderDTO> Exchanges { get; set; } = new List<ExchangeProviderDTO>();

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;
        }

        private async Task LoadDataAsync()
        {
            var getOwner = await _ownersService.GetOwner(OwnerID);

            if (getOwner.Result.IsSuccessful)
                Owner = getOwner.Result;

            LoadSelectListData();

            LoadExistingData();
        }

        private void LoadExistingData()
        {
            Input.ProviderID = Owner.ProviderID;
            
            GetSiteURL(Input.ProviderID);
        }

        private async void LoadSelectListData()
        {
            CurrentExchangeProvider.ProviderID = Owner.ProviderID;
            CurrentExchangeProvider.Name = Owner.ProviderName;
            CurrentExchangeProvider.SiteURL = Owner.ProviderSiteURL;

            Exchanges.Add(CurrentExchangeProvider);

            var getAvailableExchangeProviders = await _ownersService.GetAvailableExchangeProviders(User.Identity.Name);
            Exchanges.AddRange(getAvailableExchangeProviders.Result.ExchangeProviders);
        }

        private void GetSiteURL(int providerID)
        {
            SiteUrl = Exchanges.Where(x => x.ProviderID == providerID).First().SiteURL;
            
            StateHasChanged();
        }

        private async void OnSubmitEditExchangetAsync()
        {
            StateHasChanged();

            IsLoading = true;

            Input.OwnerID = Owner.OwnerID;

            var result = await _ownersService.UpdateExchangeOwner(Input);

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
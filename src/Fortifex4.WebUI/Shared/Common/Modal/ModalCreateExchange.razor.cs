using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Owners.Commands.CreateExchangeOwner;
using Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateExchange
    {
        public string Title { get; set; } = "New Exchange";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public BaseModal BaseModal { get; set; }

        public ClaimsPrincipal User { get; set; }

        public bool IsLoading { get; set; } = false;

        public string SiteUrl { get; set; }

        public CreateExchangeOwnerRequest Input { get; set; } = new CreateExchangeOwnerRequest();

        public string SelectedExchangeProvider
        {
            get => Input.ProviderID.ToString();
            set 
            {
                Input.ProviderID = int.Parse(value);

                GetSiteURL(int.Parse(value));
            }
        }

        public IList<ExchangeProviderDTO> Exchanges { get; set; } = new List<ExchangeProviderDTO>();

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
            var getAvailableExchangeProviders = await _ownersService.GetAvailableExchangeProviders(User.Identity.Name);
            Exchanges = getAvailableExchangeProviders.Result.ExchangeProviders.ToList();

            // default value for selecte option if user doesn't change the select option
            Input.ProviderID = Exchanges.First().ProviderID;
            GetSiteURL(Input.ProviderID);
        }

        private void GetSiteURL(int providerID)
        {
            SiteUrl = Exchanges.Where(x => x.ProviderID == providerID).First().SiteURL;

            StateHasChanged();
        }

        private async void OnSubmitCreateExchangetAsync()
        {
            StateHasChanged();

            IsLoading = true;

            Input.MemberUsername = User.Identity.Name;
            
            var result = await _ownersService.CreateExchangeOwner(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo($"/exchanges/details/{result.Result.OwnerID}");

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
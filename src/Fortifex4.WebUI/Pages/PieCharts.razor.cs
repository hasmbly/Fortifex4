using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Members.Queries.GetPortfolio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.WebUI.Pages
{
    public partial class PieCharts
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Inject]
        public IConfiguration _configuration { get; set; }

        public bool IsLoading { get; set; }

        public GetPortfolioResponse Portfolio { get; set; } = new GetPortfolioResponse();

        public IList<SelectListItem> DropdownOptions { get; set; }

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
            await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            this.DropdownOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Portfolio By Coin" },
                new SelectListItem { Value = "2", Text = "Portfolio By Exchange" },
                new SelectListItem { Value = "3", Text = "Coin By Exchange" }
            };

            Portfolio = Task.FromResult(await _portfolioService.GetPortfolio(User.Identity.Name)).Result.Result;

            IsLoading = false;

            StateHasChanged();
        }
    }

    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
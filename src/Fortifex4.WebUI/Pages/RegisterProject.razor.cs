using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages
{
    public partial class RegisterProject
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            _projectState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _projectState.OnChange -= StateHasChanged;
        }

        private void OnSubmitRegisterProject(bool isSuccessful)
        {
            if (isSuccessful)
            {
                _navigationManager.NavigateTo($"/projects/details/{_projectState.ProjectID}");
                _projectState.ResetState();
            }
        }
    }
}
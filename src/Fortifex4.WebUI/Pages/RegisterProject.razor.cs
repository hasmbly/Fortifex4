using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages
{
    public partial class RegisterProject
    {
        private bool _disposed = false;

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
                _projectState.OnChange -= StateHasChanged;
            }

            _disposed = true;
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
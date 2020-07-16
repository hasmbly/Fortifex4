﻿using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername;
using Fortifex4.Shared.Projects.Queries.GetMyProjects;
using Fortifex4.Shared.Projects.Queries.GetProjectsConfirmation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.WebUI.Pages.Projects
{
    public partial class Index
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Inject]
        public IConfiguration _configuration { get; set; }

        public bool IsLoading { get; set; }

        public bool IsAdministrator { get; set; }

        public GetMyProjectsResponse MyProjects { get; set; } = new GetMyProjectsResponse();
        public GetContributorsByMemberUsernameResponse ContributorsResult { get; set; } = new GetContributorsByMemberUsernameResponse();
        public GetProjectsConfirmationResponse ProjectsConfirmation { get; set; } = new GetProjectsConfirmationResponse();

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

        private async void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            MyProjects = Task.FromResult(await _projectsServices.GetMyProjects(User.Identity.Name)).Result.Result;

            ContributorsResult = Task.FromResult(await _projectsServices.GetContributorsByMemberUsername(User.Identity.Name)).Result.Result;

            ProjectsConfirmation = Task.FromResult(await _projectsServices.GetProjectsConfirmation()).Result.Result;

            IsAdministrator = Task.FromResult(await _devService.GetFortifexOption("FortifexAdministrator")).Result.Result == User.Identity.Name;

            IsLoading = false;

            StateHasChanged();
        }
    }
}
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Pages.Projects
{
    public partial class Details
    {
        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public int ProjectID { get; set; }

        public int ProjectDocumentsLimit { get; set; }
        public string Message { get; set; }

        public bool IsLoading { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsCreator { get; set; }
        public bool IsContributor { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public GetProjectResponse Project { get; set; } = new GetProjectResponse();
        public GetProjectStatusLogsByProjectIDResponse ProjectStatusLogs { get; set; }

        private ModalEditProject ModalEditProject { get; set; }
        private ModalUpdateProjectStatus ModalUpdateProjectStatus { get; set; }
        private ModalCreateProjectDocument ModalCreateProjectDocument { get; set; }
        private ModalInviteProjectContributor ModalInviteProjectContributor { get; set; }
        public ModalDeleteProjectContributors ModalDeleteProjectContributors { get; set; }

        protected async override Task OnInitializedAsync()
        {
            _globalState.ShouldRender += RefreshMe;
            
            _projectState.OnChange += StateHasChanged;

            await InitAsync();
        }

        public void Dispose()
        {
            _globalState.ShouldRender -= RefreshMe;

            _projectState.OnChange -= StateHasChanged;

            _projectState.ResetState();
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

            Project = Task.FromResult(await _projectsServices.GetProject(ProjectID)).Result.Result;

            IsAdministrator = Task.FromResult(await _devService.GetFortifexOption("FortifexAdministrator")).Result.Result == User.Identity.Name;

            IsCreator = Project.MemberUsername == User.Identity.Name;

            ProjectDocumentsLimit = int.Parse(Task.FromResult(await _devService.GetFortifexOption("ProjectDocumentsLimit")).Result.Result);

            if (Project.ProjectStatus == ProjectStatus.Draft)
            {
                if (!IsCreator)
                {
                    _navigationManager.NavigateTo("/projects");
                }
            }
            else if (Project.ProjectStatus == ProjectStatus.SubmittedForApproval)
            {
                if (IsCreator || IsAdministrator)
                {
                    ProjectStatusLogs = new GetProjectStatusLogsByProjectIDResponse();

                    ProjectStatusLogs = Task.FromResult(await _projectsServices.GetProjectStatusLogsByProjectID(ProjectID)).Result.Result;
                }
                else
                {
                    _navigationManager.NavigateTo("/projects");
                }
            }
            else if (Project.ProjectStatus == ProjectStatus.Returned || Project.ProjectStatus == ProjectStatus.Rejected)
            {
                if (IsCreator)
                {
                    ProjectStatusLogs = Task.FromResult(await _projectsServices.GetProjectStatusLogsByProjectID(ProjectID)).Result.Result;
                }
                else
                {
                    _navigationManager.NavigateTo("/projects");
                }
            }
            else if (Project.ProjectStatus == ProjectStatus.Approved)
            {
                IsContributor = Task.FromResult(await _projectsServices.CheckIsContributor(ProjectID, User.Identity.Name)).Result.Result.IsContributor;

                if (!IsCreator && !IsContributor)
                {
                    _navigationManager.NavigateTo("/projects");
                }
            }

            IsLoading = false;

            StateHasChanged();
        }
    }
}
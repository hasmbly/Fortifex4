using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Projects.Commands.CreateProjects;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.WebUI.Common.StateContainer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class CreateProject
    {
        private bool _disposed = false;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public bool IsRegisterProjectState { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public bool IsLoading { get; set; }

        public CreateProjectsRequest Input { get; set; } = new CreateProjectsRequest();

        public GetProjectResponse Project { get; set; } = new GetProjectResponse();

        public DotNetObjectReference<ProjectState> ProjectState { get; set; }

        public const string ElementID = "select-blockchain-project";

        public string SelectedBlockchains
        {
            get => Input.BlockchainID.ToString();
            set
            {
                Input.BlockchainID = int.Parse(value);
            }
        }

        public IList<BlockchainDTO> Blockchains { get; set; } = new List<BlockchainDTO>();

        protected async override Task OnInitializedAsync() 
        {
            _projectState.OnChangeBlockchainID += SetBlockchainID;

            await InitAsync();
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
                _projectState.OnChangeBlockchainID -= SetBlockchainID;

                ProjectState?.Dispose();
            }

            _disposed = true;
        }

        private async Task InitAsync()
        {
            _projectState.SetIsLoading(true);

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            if (User.Identity.IsAuthenticated)
            {
                Input.MemberUsername = User.Identity.Name;

                var getProjectIsExist = await _projectsServices.GetProjectIsExist(User.Identity.Name);

                if (getProjectIsExist.Result.IsExistProjectByMemberUsernameResult)
                {
                    _projectState.SetIsUserHasProject(true, getProjectIsExist.Result.ProjectID);
                    _projectState.SetMessage("danger", "Sorry, you already have a Project");
                }
            }

            await LoadBlockchains();

            _projectState.SetIsLoading(false);

            StateHasChanged();

            FirstStage = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }
            else if (FirstStage)
            {
                FirstStage = false;

                ProjectState = DotNetObjectReference.Create(_projectState);

                await JsRuntime.InvokeVoidAsync("ChoosenSelect.initWithOnChange", $"#{ElementID}", ProjectState);
            }
        }

        private async Task LoadBlockchains()
        {
            var getProjectIsExist = await _blockchainsService.GetAllBlockchains();

            if (getProjectIsExist.Result.IsSuccessful)
                Blockchains = getProjectIsExist.Result.Blockchains;
        }

        private void SetBlockchainID(int blockchainID)
        {
            Input.BlockchainID = blockchainID;

            Console.WriteLine($"Blazor: SetBlockchainID: {Input.BlockchainID}");
        }

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var result = await _projectsServices.CreateProject(Input);

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _projectState.SetProjectID(result.Result.ProjectID);
                    await OnAfterSuccessful.InvokeAsync(true);
                    IsLoading = false;
                }
                else
                {
                    Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
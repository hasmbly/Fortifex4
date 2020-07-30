using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.WebUI.Common.StateContainer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditProject
    {
        private bool _disposed = false;

        public string Title { get; set; } = "Edit Project";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int ProjectID { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public bool IsLoading { get; set; }
        
        public BaseModal BaseModal { get; set; }

        public UpdateProjectsRequest Input { get; set; } = new UpdateProjectsRequest();

        public GetProjectResponse Project { get; set; } = new GetProjectResponse();

        public DotNetObjectReference<ProjectState> ProjectState { get; set; }

        public const string ElementID = "select-blockchain-edit-project";

        public string SelectedBlockchains
        {
            get => Input.ProjectBlockchainID.ToString();
            set
            {
                Input.ProjectBlockchainID = int.Parse(value);
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
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadExistingData();
            await LoadBlockchains();

            IsLoading = false;

            StateHasChanged();

            FirstStage = true;
        }

        private async Task LoadExistingData()
        {
            Project = Task.FromResult(await _projectsServices.GetProject(ProjectID)).Result.Result;

            Input.ProjectID = ProjectID;
            Input.ProjectName = Project.Name;
            Input.ProjectBlockchainID = Project.BlockchainID;
            Input.ProjectWalletAddress = Project.WalletAddress;
            Input.ProjectDescription = Project.Description;
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
                Blockchains = getProjectIsExist.Result.Blockchains.ToList();
        }

        private void SetBlockchainID(int blockchainID)
        {
            Input.ProjectBlockchainID = blockchainID;
        }

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var result = await _projectsServices.UpdateProject(Input);

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    await OnAfterSuccessful.InvokeAsync(true);

                    IsLoading = false;

                    BaseModal.Close();
                }
                else
                {
                    Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}
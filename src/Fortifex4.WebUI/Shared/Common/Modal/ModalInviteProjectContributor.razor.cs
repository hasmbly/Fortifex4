using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Contributors.Commands.CreateContributors;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using Fortifex4.Shared.Projects.Queries.GetProject;
using Fortifex4.WebUI.Common.StateContainer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.ObjectPool;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalInviteProjectContributor
    {
        public string Title { get; set; } = "Invite Contributors";

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int ProjectID { get; set; }

        [Parameter]
        public string ProjectName { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public bool IsLoading { get; set; }

        public BaseModal BaseModal { get; set; }

        public CreateContributorsRequest Input { get; set; } = new CreateContributorsRequest();

        public const string ElementID = "select-members-name";

        protected async override Task OnInitializedAsync()
        {
            await InitAsync();
        }

        public void Dispose()
        {
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            Input.ProjectID = ProjectID;

            IsLoading = false;

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

                await JsRuntime.InvokeVoidAsync("EmailMultiple.init", $"#{ElementID}");
            }
        }

        private async Task<List<string>> GetListOfContributorAsync() 
        {
           return await JsRuntime.InvokeAsync<List<string>>("EmailMultiple.getListOfContributor", $"#{ElementID}");
        }

        private async Task SubmitAsync()
        {
            IsLoading = true;

            Input.MemberUsername = Task.FromResult(await GetListOfContributorAsync()).Result;

            var result = await _projectsServices.InviteMembers(Input);

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
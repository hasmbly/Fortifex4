using System;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Projects.Commands.UpdateProjectStatus;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalUpdateProjectStatus
    {
        public string Title { get; set; } = "Request for Approval";

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int ProjectID { get; set; }

        public bool IsLoading { get; set; }

        public BaseModal BaseModal { get; set; }

        public UpdateProjectStatusRequest Input { get; set; } = new UpdateProjectStatusRequest();

        protected override void OnInitialized()
        {
            InitAsync();
        }

        private void InitAsync()
        {
            IsLoading = true;

            Input.ProjectID = ProjectID;
            
            IsLoading = false;

            StateHasChanged();
        }

        private void SetProjectStatus(ProjectStatus projectStatus)
        {
            Input.NewProjectStatus = projectStatus;

            Title = Input.NewProjectStatus switch
            {
                ProjectStatus.Approved => "Approve Project",
                ProjectStatus.Returned => "Return Project",
                ProjectStatus.Rejected => "Reject Project",
                _ => "Request for Approval"
            };

            StateHasChanged();
        }

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var result = await _projectsServices.UpdateProjectStatus(Input);

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    if (Input.NewProjectStatus == ProjectStatus.SubmittedForApproval)
                        _projectState.SetMessage("danger", "Project is submitted for Approval");
                    else
                        _navigationManager.NavigateTo("/projects");

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
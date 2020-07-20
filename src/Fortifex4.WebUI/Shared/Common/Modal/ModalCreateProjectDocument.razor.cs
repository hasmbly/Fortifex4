using System;
using System.Threading.Tasks;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.ProjectDocuments.Commands.CreateProjectDocument;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreateProjectDocument
    {
        public string Title { get; set; } = "Upload File Document Project";

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int ProjectID { get; set; }

        public bool IsLoading { get; set; }

        public BaseModal BaseModal { get; set; }

        public CreateProjectDocumentRequest Input { get; set; } = new CreateProjectDocumentRequest();

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

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var result = await _projectsDocumentService.CreateProjectDocument(Input);

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
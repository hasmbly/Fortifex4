using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorInputFile;
using Fortifex4.Shared.ProjectDocuments.Commands.UpdateProjectDocument;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditProjectDocument
    {
        public string Title { get; set; } = "Edit Upload File Document Project";

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public int ProjectDocumentID { get; set; }

        public bool IsLoading { get; set; }

        public BaseModal BaseModal { get; set; }

        public UpdateProjectDocumentRequest Input { get; set; } = new UpdateProjectDocumentRequest();

        public IFileListEntry File { get; set; }

        private void SetProjectDocumentID(int projectDocumentID) => Input.ProjectDocumentID = projectDocumentID;

        private void HandleFileSelected(IFileListEntry[] files)
        {
            File = files.FirstOrDefault();
        }

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var content = new MultipartFormDataContent();

            if (File != null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");

                content.Add(new StringContent(Input.ProjectDocumentID.ToString()), "ProjectDocumentID");
                content.Add(new StringContent(Input.Title), "Title");
                content.Add(new StreamContent(File.Data, (int)File.Data.Length), "FormFileProjectDocument", File.Name);
            }

            var result = await _projectsDocumentService.UpdateProjectDocument(content);

            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine($"IsError: {result.Content}");
            }
            else
            {
                Console.WriteLine($"IsError: {result.Content}");

                await OnAfterSuccessful.InvokeAsync(true);

                IsLoading = false;

                BaseModal.Close();
            }
        }
    }
}
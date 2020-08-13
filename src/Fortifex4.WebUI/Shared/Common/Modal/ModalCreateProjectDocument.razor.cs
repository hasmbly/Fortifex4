using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BlazorInputFile;
using Fortifex4.Shared.ProjectDocuments.Commands.CreateProjectDocument;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

        public IFileListEntry File { get; set; }

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

        private void HandleFileSelected(IFileListEntry[] files)
        {
            File = files.FirstOrDefault();
        }

        private async void ResetInputFile() => await JsRuntime.InvokeVoidAsync("BlazorInputFile.reset");

        private async Task SubmitAsync()
        {
            IsLoading = true;

            var content = new MultipartFormDataContent();

            if (File != null)
            {
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");

                content.Add(new StringContent(Input.ProjectID.ToString()), "ProjectID");
                content.Add(new StringContent(Input.Title), "Title");
                content.Add(new StreamContent(File.Data, (int)File.Data.Length), "FormFileProjectDocument", File.Name);
            }

            var result = await _projectsDocumentService.CreateProjectDocument(content);

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
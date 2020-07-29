using System;
using System.Collections;
using Fortifex4.Shared.ProjectDocuments.Commands.DeleteProjectDocument;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalDeleteProjectDocument
    {
        public BaseModal BaseModal { get; set; }

        public string Title { get; set; } = "Delete Projects Document";

        public string Message { get; set; } = "Projects Document";

        public int ID { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public bool IsLoading { get; set; } = false;

        private void SetID(int value) => ID = value;

        private void SetDataAndID(Hashtable ht)
        {
            foreach (DictionaryEntry kv in ht)
            {
                if (kv.Key.ToString() == "id")
                {
                    ID = (int)kv.Value;
                }
                else if (kv.Key.ToString() == "data")
                {
                    Message = kv.Value.ToString();
                }
            }
        }

        private async void OnSubmitAsync()
        {
            IsLoading = true;

            var result = await _projectsDocumentService.DeleteProjectDocument(new DeleteProjectDocumentRequest() { ProjectDocumentID = ID });

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    IsLoading = false;

                    await OnAfterSuccessful.InvokeAsync(true);

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
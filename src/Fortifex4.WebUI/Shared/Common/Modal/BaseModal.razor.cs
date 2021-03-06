﻿using System.Collections;
using Fortifex4.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class BaseModal
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string ModalTitle { get; set; } = "Default Title";

        [Parameter]
        public bool IsLoading { get; set; }

        [Parameter]
        public string ModalSize { get; set; } = "modal-lg";

        [Parameter]
        public EventCallback OnSubmit { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback<int> IsOpenWithID { get; set; }

        [Parameter]
        public EventCallback<Hashtable> IsOpenWithDataAndID { get; set; }

        [Parameter]
        public EventCallback<ProjectStatus> IsOpenWithProjectStatus { get; set; }

        [Parameter]
        public EventCallback TriggerAfterModalOpened { get; set; }

        [Parameter]
        public string ConfirmationWord { get; set; } = "Submit";

        [Parameter]
        public string CancellationWord { get; set; } = "Cancel";

        public bool ShowBackdrop { get; set; }

        public string ModalDisplay { get; set; } = "none;";

        public string ModalClass { get; set; }

        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "show";
            ShowBackdrop = true;

            TriggerAfterModalOpened.InvokeAsync(null);

            StateHasChanged();
        }
        
        public void OpenWithID(int id)
        {
            IsOpenWithID.InvokeAsync(id);

            Open();
        }

        public void OpenWithDataAndID(int id, object data)
        {
            Hashtable ht = new Hashtable
            {
                { "id", id },
                { "data", data }
            };

            IsOpenWithDataAndID.InvokeAsync(ht);

            Open();
        }

        public void OpenWithProjectStatus(ProjectStatus projectStatus)
        {
            IsOpenWithProjectStatus.InvokeAsync(projectStatus);

            Open();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = string.Empty;
            ShowBackdrop = false;

            OnClose.InvokeAsync(null);

            StateHasChanged();
        }

        public void SetConfirmationWord(string words)
        {
            ConfirmationWord = words;

            StateHasChanged();
        }

        public void SetCancellationWord(string words)
        {
            CancellationWord = words;

            StateHasChanged();
        }
    }
}
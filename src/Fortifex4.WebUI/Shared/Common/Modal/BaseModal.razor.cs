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
        public EventCallback OnSubmit { get; set; }

        [Parameter]
        public EventCallback<int> IsOpenWithID { get; set; }

        public bool ShowBackdrop { get; set; }

        public string ModalDisplay { get; set; } = "none;";

        public string ModalClass { get; set; }

        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "show";
            ShowBackdrop = true;

            StateHasChanged();
        }

        public void OpenWithID(int id)
        {
            IsOpenWithID.InvokeAsync(id);

            Open();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = string.Empty;
            ShowBackdrop = false;

            StateHasChanged();
        }
    }
}
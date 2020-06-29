using System.Collections.Generic;
using System.Threading.Tasks;
using Fortifex4.WebUI.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class ToggleCheckbox
    {
        [Parameter]
        public Dictionary<string, object> Attributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "id", "external-transfer-toggle-direction" },
                { "class", "form-check-input" },
                { "type", "checkbox" },
                { "checked", null },
                { "data-toggle", "toggle" },
                { "data-on", "IN" },
                { "data-off", "OUT" },
                { "data-onstyle", "primary" },
                { "data-offstyle", "secondary" },
                { "data-width", "100%" },
                { "data-height", "38" },
            };

        public DotNetObjectReference<ToggleCheckboxState> ToggleCheckboxState { get; set; }

        protected override void OnInitialized()
        {
            System.Console.WriteLine("ToggleCheckbox - OnInitialized");

            _toggleCheckboxState.SetDefaultIsChecked();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            System.Console.WriteLine("ToggleCheckbox - OnAfterRender");

            if (firstRender)
            {
                System.Console.WriteLine("ToggleCheckbox - OnAfterRender - firstRender");

                ToggleCheckboxState = DotNetObjectReference.Create(_toggleCheckboxState);

                await JsRuntime.InvokeVoidAsync("Toggle.init");

                await JsRuntime.InvokeVoidAsync("Toggle.onChangeToggle", "#external-transfer-toggle-direction", ToggleCheckboxState);
            }
        }
    }
}
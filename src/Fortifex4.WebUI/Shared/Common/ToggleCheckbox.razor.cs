using System;
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
        public ToggleCheckboxAttributes Attributes { get; set; }

        public DotNetObjectReference<ToggleCheckboxState> ToggleCheckboxState { get; set; }

        protected override void OnInitialized()
        {
            _toggleCheckboxState.SetDefaultIsChecked();

            _toggleCheckboxState.SetToggleChange += SetToggle;
            _toggleCheckboxState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _toggleCheckboxState.SetDefaultIsChecked();

            _toggleCheckboxState.SetToggleChange -= SetToggle;
            _toggleCheckboxState.OnChange -= StateHasChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ToggleCheckboxState = DotNetObjectReference.Create(_toggleCheckboxState);

                await JsRuntime.InvokeVoidAsync("Toggle.onChangeToggle", $"#{Attributes.ElementID}", ToggleCheckboxState);
            }
        }

        public async void SetToggle(string elementID, bool isChecked)
        {
            Console.WriteLine($"SetToggle from ToggleCheckbox : elementID = {elementID}, isChecked = {isChecked}");

            await JsRuntime.InvokeVoidAsync("Toggle.setToggle", $"#{elementID}", isChecked);
        }

        public class ToggleCheckboxAttributes
        {
            public string ElementID { get; set; } = "";
            
            public Dictionary<string, object> ElementAttributes { get; set; }

            public ToggleCheckboxAttributes(string _elementID)
            {
                ElementID = _elementID;

                ElementAttributes = new Dictionary<string, object>()
                {
                    { "id", ElementID },
                    { "class", "form-check-input" },
                    { "type", "checkbox" },
                    { "checked", "" },
                    { "data-toggle", "toggle" },
                    { "data-on", "IN" },
                    { "data-off", "OUT" },
                    { "data-onstyle", "primary" },
                    { "data-offstyle", "secondary" },
                    { "data-width", "100%" },
                    { "data-height", "38" },
                };
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Fortifex4.WebUI.Common;
using Fortifex4.WebUI.Common.StateContainer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common
{
    public partial class ToggleCheckbox
    {
        [Parameter]
        public ToggleCheckboxAttributes Attributes { get; set; }

        [Parameter]
        public EventCallback<string> OnChangeChecked { get; set; }

        public DotNetObjectReference<ToggleCheckboxState> ToggleCheckboxState { get; set; }

        protected override void OnInitialized()
        {
            _toggleCheckboxState.SetDefaultIsChecked();

            _toggleCheckboxState.SetToggleChange += SetToggle;
            _toggleCheckboxState.ToggleHasChanged += ToggleHasChanged;
            _toggleCheckboxState.SetTogglePropChange += SetToggleProp;

            _toggleCheckboxState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            _toggleCheckboxState.SetDefaultIsChecked();

            _toggleCheckboxState.SetToggleChange -= SetToggle;
            _toggleCheckboxState.ToggleHasChanged -= ToggleHasChanged;
            _toggleCheckboxState.SetTogglePropChange -= SetToggleProp;

            _toggleCheckboxState.OnChange -= StateHasChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ToggleCheckboxState = DotNetObjectReference.Create(_toggleCheckboxState);

                await JsRuntime.InvokeVoidAsync("Toggle.onChangeToggle", $"#{Attributes.Value.ElementID}", ToggleCheckboxState);
            }
        }

        private async void ToggleHasChanged(string elementID)
        {
            await OnChangeChecked.InvokeAsync(elementID);
        }

        public async void SetToggle(string elementID, bool isChecked)
        {
            await JsRuntime.InvokeVoidAsync("Toggle.setToggle", $"#{elementID}", isChecked);
        }

        public async void SetToggleProp(string elementID, string propName, bool propState)
        {
            await JsRuntime.InvokeVoidAsync("Toggle.setToggleProp", $"#{elementID}", propName, propState);
        }

        public class ToggleCheckboxAttributes
        {
            public ToggleCheckboxAttributesValue Value { get; set; }

            public Dictionary<string, object> ElementAttributes { get; set; }

            public ToggleCheckboxAttributes(ToggleCheckboxAttributesValue _value)
            {
                Value = _value;

                ElementAttributes = new Dictionary<string, object>()
                {
                    { "id", Value.ElementID },
                    { "class", "form-check-input" },
                    { "type", "checkbox" },
                    { "checked", "" },
                    { "data-toggle", "toggle" },
                    { "data-on", Value.DataOn },
                    { "data-off", Value.DataOff },
                    { "data-onstyle", Value.DataOnStyle },
                    { "data-offstyle", Value.DataOffStyle },
                    { "data-width", "100%" },
                    { "data-height", "38" },
                };
            }
        }
    }
}
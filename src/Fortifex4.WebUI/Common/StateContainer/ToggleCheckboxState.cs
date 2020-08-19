using System;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Common.StateContainer
{
    public class ToggleCheckboxState
    {
        public bool IsChecked { get; private set; }

        public string LabelAmount { get; set; } = "Incoming Amount";

        public event Action OnChange;

        public event Action<string, bool> SetToggleChange;

        public event Action<bool> DirectionHasChange;

        public event Action<string> ToggleHasChanged;

        public event Action<string, string, bool> SetTogglePropChange;

        [JSInvokable]
        public void SetIsChecked(string elementID, bool isChecked)
        {
            if (isChecked)
                LabelAmount = "Incoming Amount";
            else
                LabelAmount = "Outgoing Amount";

            IsChecked = isChecked;

            DirectionHasChange.Invoke(isChecked);

            ToggleHasChanged.Invoke(elementID);

            NotifyStateChanged();
        }

        public void SetDefaultState()
        {
            IsChecked = true;
            LabelAmount = "Incoming Amount";

            NotifyStateChanged();
        }

        public void SetToggle(string elementID, bool isChecked)
        {
            SetToggleChange.Invoke(elementID, isChecked);
        }

        public void SetToggleProp(string elementID, string propName, bool propState)
        {
            SetTogglePropChange.Invoke(elementID, propName, propState);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
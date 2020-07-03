using System;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Common.StateContainer
{
    public class ToggleCheckboxState
    {
        public bool IsChecked { get; private set; }

        public string LabelAmount { get; set; }

        public event Action OnChange;

        public event Action<string, bool> SetToggleChange;

        [JSInvokable]
        public void SetIsChecked(bool isChecked)
        {
            if (isChecked)
                LabelAmount = "Incoming Amount";
            else
                LabelAmount = "Outgoing Amount";

            IsChecked = isChecked;

            NotifyStateChanged();

            Console.WriteLine($"SetIsChecked from Blazor: {IsChecked}");

            Console.WriteLine($"LabelAmount from Blazor: {IsChecked}");
        }

        public void SetDefaultIsChecked()
        {
            IsChecked = true;

            NotifyStateChanged();
        }

        public void SetToggle(string elementID, bool isChecked)
        {
            SetToggleChange.Invoke(elementID, isChecked);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
using System;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Common
{
    public class ToggleCheckboxState
    {
        public bool IsChecked { get; private set; }

        public event Action OnChange;

        [JSInvokable]
        public void SetIsChecked(bool isChecked)
        {
            IsChecked = isChecked;

            NotifyStateChanged();

            Console.WriteLine($"SetIsChecked: {IsChecked}");
        }

        public void SetDefaultIsChecked()
        {
            IsChecked = false;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
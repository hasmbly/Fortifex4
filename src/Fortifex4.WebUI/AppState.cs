using System;

namespace Fortifex4.WebUI
{
    public class AppState
    {
        public bool IsHasHeader { get; private set; }

        public event Action OnChange;

        public void SetIsHasHeader(bool state)
        {
            IsHasHeader = state;
            NotifyStateChanged();
        }

        public bool GetIsHasHeader()
        {
            return IsHasHeader;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
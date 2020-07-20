using System;

namespace Fortifex4.WebUI.Common.StateContainer
{
    public class WalletTransactionsState
    {
        public event Action ShouldRender;

        public void NotifyStateChanged() => ShouldRender?.Invoke();
    }
}
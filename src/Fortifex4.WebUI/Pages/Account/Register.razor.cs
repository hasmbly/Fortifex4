using System;

namespace Fortifex4.WebUI.Pages.Account
{
    public partial class Register
    {
        private bool _disposed = false;

        protected override void OnInitialized()
        {
            activateMemberState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                activateMemberState.OnChange -= StateHasChanged;
            }

            _disposed = true;
        }
    }
}
using System;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Common.StateContainer
{
    public class ProjectState
    {
        public string Message { get; private set; }

        public string ExistMemberUsername { get; private set; }

        public bool IsUserHasProject { get; private set; }

        public int? ProjectID { get; private set; }

        public bool IsLoading { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public event Action OnChange;

        public event Action<int> OnChangeBlockchainID;

        [JSInvokable]
        public void SetBlockchainID(string blockchainID)
        {
            OnChangeBlockchainID.Invoke(int.Parse(blockchainID));
        }

        public void SetIsUserHasProject(bool isUserHasProject, int? projectID)
        {
            IsUserHasProject = isUserHasProject;

            ProjectID = projectID.Value;

            NotifyStateChanged();
        }

        public void SetIsAuthenticated(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;

            NotifyStateChanged();
        }

        public void SetMessage(string message)
        {
            Message = message;

            NotifyStateChanged();
        }

        public void SetExistMemberUsername(string existMemberUsername)
        {
            ExistMemberUsername = existMemberUsername;

            NotifyStateChanged();
        }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;

            NotifyStateChanged();
        }

        public void ResetState()
        {
            Message = string.Empty;
            ExistMemberUsername = string.Empty;
            IsUserHasProject = false;
            ProjectID = null;
            IsLoading = false;
            IsAuthenticated = false;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
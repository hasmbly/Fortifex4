using System;
using Fortifex4.Shared.Members.Commands.CreateMember;

namespace Fortifex4.WebUI.Common.StateContainer
{
    public class ActivateMemberState
    {
        public bool ShowActivationMessage { get; private set; }

        public bool ShowActivationSuccesMessage { get; private set; }

        public CreateMemberResponse Member { get; set; } = new CreateMemberResponse();

        public event Action OnChange;

        public void SetActivateMemberState(CreateMemberResponse createMemberResponse)
        {
            Member.ActivationCode = createMemberResponse.ActivationCode;    
            Member.MemberUsername = createMemberResponse.MemberUsername;

            ShowActivationMessage = true;

            NotifyStateChanged();
        }

        public void DoneActivateMemberState()
        {
            ShowActivationSuccesMessage = true;

            ShowActivationMessage = false;

            NotifyStateChanged();
        }

        public void ResetActivateMemberState()
        {
            Member = new CreateMemberResponse();

            ShowActivationMessage = false;

            ShowActivationSuccesMessage = false;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
namespace Fortifex4.Shared.Members.Commands.ActivateMember
{
    public class ActivateMemberResponse
    {
        public string MemberUsername { get; set; }

        public bool IsSuccessful => !string.IsNullOrEmpty(this.MemberUsername);
    }
}
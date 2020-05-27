namespace Fortifex4.Application.Members.Commands.ActivateMember
{
    public class ActivateMemberResult
    {
        public string MemberUsername { get; set; }

        public bool IsSuccessful => !string.IsNullOrEmpty(this.MemberUsername);
    }
}
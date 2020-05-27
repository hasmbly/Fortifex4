namespace Fortifex4.Application.Members.Queries.Login
{
    public class LoginResult
    {
        public bool UsernameExists { get; set; }
        public bool UsingFortifexAuthentication { get; set; }
        public bool PasswordIsCorrect { get; set; }
        public bool AccountIsActive { get; set; }
        public int? ProjectID { get; set; }

        public bool DoesMemberHasProject => this.ProjectID.HasValue;
    }
}
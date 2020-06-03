using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Members.Queries.Login
{
    public class LoginResponse : GeneralResponse
    {
        public bool UsernameExists { get; set; }
        public bool UsingFortifexAuthentication { get; set; }
        public bool PasswordIsCorrect { get; set; }
        public bool AccountIsActive { get; set; }
        public int? ProjectID { get; set; }

        public bool DoesMemberHasProject => this.ProjectID.HasValue;
     
        public string Token { get; set; }
    }
}
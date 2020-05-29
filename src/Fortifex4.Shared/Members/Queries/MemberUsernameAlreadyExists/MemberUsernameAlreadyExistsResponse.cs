using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Members.Queries.MemberUsernameAlreadyExists
{
    public class MemberUsernameAlreadyExistsResponse : GeneralResponse
    {
        public bool DoesMemberExist { get; set; }
    }
}
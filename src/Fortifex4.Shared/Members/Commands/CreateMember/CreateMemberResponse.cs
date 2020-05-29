using System;

namespace Fortifex4.Shared.Members.Commands.CreateMember
{
    public class CreateMemberResponse
    {
        public bool IsSuccessful { get; set; }
        public Guid ActivationCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
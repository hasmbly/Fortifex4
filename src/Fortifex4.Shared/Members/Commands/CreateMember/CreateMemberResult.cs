using System;

namespace Fortifex4.Application.Members.Commands.CreateMember
{
    public class CreateMemberResult
    {
        public bool IsSuccessful { get; set; }
        public Guid ActivationCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
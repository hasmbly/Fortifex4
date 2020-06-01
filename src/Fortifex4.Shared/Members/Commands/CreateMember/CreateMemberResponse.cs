using System;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Members.Commands.CreateMember
{
    public class CreateMemberResponse : GeneralResponse
    {
        public Guid ActivationCode { get; set; }
    }
}
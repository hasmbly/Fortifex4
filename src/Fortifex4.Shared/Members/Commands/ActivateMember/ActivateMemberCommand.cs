using System;
using MediatR;

namespace Fortifex4.Shared.Members.Commands.ActivateMember
{
    public class ActivateMemberRequest : IRequest<ActivateMemberResponse>
    {
        public Guid ActivationCode { get; set; }
    }
}
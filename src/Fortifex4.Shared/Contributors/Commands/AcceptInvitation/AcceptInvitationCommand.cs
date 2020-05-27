using MediatR;

namespace Fortifex4.Application.Contributors.Commands.AcceptInvitation
{
    public class AcceptInvitationCommand : IRequest<AcceptInvitationResult>
    {
        public string InvitationCode { get; set; }
    }
}
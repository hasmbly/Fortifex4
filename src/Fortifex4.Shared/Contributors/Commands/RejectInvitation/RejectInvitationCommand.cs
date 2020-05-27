using MediatR;

namespace Fortifex4.Application.Contributors.Commands.RejectInvitation
{
    public class RejectInvitationCommand : IRequest<RejectInvitationResult>
    {
        public string InvitationCode { get; set; }
    }
}
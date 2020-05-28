using MediatR;

namespace Fortifex4.Shared.Contributors.Commands.RejectInvitation
{
    public class RejectInvitationRequest : IRequest<RejectInvitationResponse>
    {
        public string InvitationCode { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Shared.Contributors.Commands.AcceptInvitation
{
    public class AcceptInvitationRequest : IRequest<AcceptInvitationResponse>
    {
        public string InvitationCode { get; set; }
    }
}
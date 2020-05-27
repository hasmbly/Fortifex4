using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Application.Contributors.Commands.UpdateContributorInvitationStatus
{
    public class UpdateContributorInvitationStatusCommand : IRequest<UpdateContributorInvitationStatusResult>
    {
        public InvitationStatus InvitationStatus { get; set; }
        public int ContributorID { get; set; }
    }
}
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.Contributors.Commands.UpdateContributorInvitationStatus
{
    public class UpdateContributorInvitationStatusRequest : IRequest<UpdateContributorInvitationStatusResponse>
    {
        public InvitationStatus InvitationStatus { get; set; }
        public int ContributorID { get; set; }
    }
}
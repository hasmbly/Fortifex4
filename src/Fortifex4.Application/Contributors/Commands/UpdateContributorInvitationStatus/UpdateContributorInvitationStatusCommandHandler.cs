using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Contributors.Commands.UpdateContributorInvitationStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.UpdateContributorInvitationStatus
{
    public class UpdateContributorInvitationStatusCommandHandler : IRequestHandler<UpdateContributorInvitationStatusRequest, UpdateContributorInvitationStatusResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateContributorInvitationStatusCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }
        public async Task<UpdateContributorInvitationStatusResponse> Handle(UpdateContributorInvitationStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateContributorInvitationStatusResponse()
            {
                IsSuccessful = false
            };

            var contributor = await _context.Contributors
                .Where(x => x.ContributorID == request.ContributorID)
                .SingleOrDefaultAsync(cancellationToken);

            if (contributor == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.ContributorNotFound;

                return result;
            }

            contributor.InvitationStatus = request.InvitationStatus;

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
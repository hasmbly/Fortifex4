using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.UpdateContributorInvitationStatus
{
    public class UpdateContributorInvitationStatusCommandHandler : IRequestHandler<UpdateContributorInvitationStatusCommand, UpdateContributorInvitationStatusResult>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateContributorInvitationStatusCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }
        public async Task<UpdateContributorInvitationStatusResult> Handle(UpdateContributorInvitationStatusCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateContributorInvitationStatusResult()
            {
                IsSuccessful = false
            };

            var contributor = await _context.Contributors
                .Where(x => x.ContributorID == request.ContributorID)
                .SingleOrDefaultAsync(cancellationToken);

            if (contributor == null)
                throw new NotFoundException(nameof(contributor), request.ContributorID);

            contributor.InvitationStatus = request.InvitationStatus;

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
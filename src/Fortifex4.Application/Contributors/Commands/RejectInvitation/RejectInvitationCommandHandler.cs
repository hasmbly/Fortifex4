using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Contributors.Commands.RejectInvitation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.RejectInvitation
{
    public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationRequest, RejectInvitationResponse>
    {
        private readonly IFortifex4DBContext _context;

        public RejectInvitationCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<RejectInvitationResponse> Handle(RejectInvitationRequest query, CancellationToken cancellationToken)
        {
            var result = new RejectInvitationResponse();

            var contributor = await _context.Contributors
                .Where(x => x.InvitationCode == new Guid(query.InvitationCode))
                .SingleOrDefaultAsync();

            if (contributor != null)
            {
                if (contributor.InvitationStatus == InvitationStatus.Invited)
                {
                    contributor.InvitationStatus = InvitationStatus.Rejected;

                    await _context.SaveChangesAsync(cancellationToken);

                    result.IsSuccessful = true;
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.ProjectNotFound;
            }

            return result;
        }
    }
}
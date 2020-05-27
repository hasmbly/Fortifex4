using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.RejectInvitation
{
    public class RejectInvitationCommandHandler : IRequestHandler<RejectInvitationCommand, RejectInvitationResult>
    {
        private readonly IFortifex4DBContext _context;

        public RejectInvitationCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<RejectInvitationResult> Handle(RejectInvitationCommand query, CancellationToken cancellationToken)
        {
            RejectInvitationResult result = new RejectInvitationResult();

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

            return result;
        }
    }
}
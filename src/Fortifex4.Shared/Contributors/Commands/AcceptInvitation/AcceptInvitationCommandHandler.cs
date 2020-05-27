using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, AcceptInvitationResult>
    {
        private readonly IFortifex4DBContext _context;

        public AcceptInvitationCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<AcceptInvitationResult> Handle(AcceptInvitationCommand query, CancellationToken cancellationToken)
        {
            AcceptInvitationResult result = new AcceptInvitationResult();

            var contributor = await _context.Contributors
                .Where(x => x.InvitationCode == new Guid(query.InvitationCode))
                .SingleOrDefaultAsync();

            if (contributor != null)
            {
                if (contributor.InvitationStatus == InvitationStatus.Invited)
                {
                    contributor.InvitationStatus = InvitationStatus.Accepted;

                    await _context.SaveChangesAsync(cancellationToken);

                    result.IsSuccessful = true;
                }
            }

            return result;
        }
    }
}
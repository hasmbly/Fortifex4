using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.ActivateMember
{
    public class ActivateMemberCommand : IRequest<ActivateMemberResult>
    {
        public Guid ActivationCode { get; set; }
    }

    public class ActivateMemberCommandHandler : IRequestHandler<ActivateMemberCommand, ActivateMemberResult>
    {
        private readonly IFortifex4DBContext _context;

        public ActivateMemberCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<ActivateMemberResult> Handle(ActivateMemberCommand query, CancellationToken cancellationToken)
        {
            var result = new ActivateMemberResult();

            var member = await _context.Members
                .Where(x => x.ActivationCode == query.ActivationCode)
                .SingleOrDefaultAsync(cancellationToken);

            if (member != null)
            {
                member.ActivationStatus = ActivationStatus.Active;

                await _context.SaveChangesAsync(cancellationToken);

                result.MemberUsername = member.MemberUsername;
            }

            return result;
        }
    }
}
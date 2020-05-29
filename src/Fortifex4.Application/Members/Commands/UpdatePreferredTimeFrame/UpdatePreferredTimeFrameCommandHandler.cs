using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.UpdatePreferredTimeFrame
{
    public class UpdatePreferredTimeFrameCommandHandler : IRequestHandler<UpdatePreferredTimeFrameRequest, UpdatePreferredTimeFrameResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdatePreferredTimeFrameCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdatePreferredTimeFrameResponse> Handle(UpdatePreferredTimeFrameRequest request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            member.PreferredTimeFrameID = request.PreferredTimeFrameID;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePreferredTimeFrameResponse 
            {
                IsSucessful = true
            };
        }
    }
}
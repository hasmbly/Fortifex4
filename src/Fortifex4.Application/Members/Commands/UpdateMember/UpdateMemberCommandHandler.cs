using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Members.Commands.UpdateMember;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.UpdateMember
{
    public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberRequest, UpdateMemberResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateMemberCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateMemberResponse> Handle(UpdateMemberRequest request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            member.FirstName = request.FirstName;
            member.LastName = request.LastName;
            member.BirthDate = request.BirthDate;
            member.RegionID = request.RegionID;
            member.GenderID = request.GenderID;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateMemberResponse
            {
                IsSucessful = true
            };
        }
    }
}
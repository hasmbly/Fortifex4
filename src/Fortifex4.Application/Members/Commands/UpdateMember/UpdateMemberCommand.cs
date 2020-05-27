using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.UpdateMember
{
    public class UpdateMemberCommand : IRequest
    {
        public string MemberUsername { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }
        public int RegionID { get; set; }
    }

    public class UpdateMemberCommandHandler : IRequestHandler<UpdateMemberCommand>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateMemberCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
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

            return Unit.Value;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Lookup.Queries.GetOwners
{
    public class GetOwnersQueryHandler : IRequestHandler<GetOwnersQuery, GetOwnersResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetOwnersQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetOwnersResult> Handle(GetOwnersQuery query, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == query.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            var owners = await _context.Owners
                .Where(x => x.MemberUsername == query.MemberUsername && x.ProviderType == ProviderType.Exchange)
                .Include(a => a.Provider)
                .ToListAsync(cancellationToken);

            List<OwnerDTO> ownerDTOs = new List<OwnerDTO>();

            foreach (var owner in owners)
            {
                OwnerDTO ownerDTO = new OwnerDTO
                {
                    OwnerID = owner.OwnerID,
                    ProviderName = owner.Provider.Name
                };

                ownerDTOs.Add(ownerDTO);
            }

            return new GetOwnersResult
            {
                Owners = ownerDTOs
            };
        }
    }
}
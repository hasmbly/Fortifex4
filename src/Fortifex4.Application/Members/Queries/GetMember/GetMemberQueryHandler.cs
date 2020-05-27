using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Queries.GetMember
{
    public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, GetMemberResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetMemberQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetMemberResult> Handle(GetMemberQuery query, CancellationToken cancellationToken)
        {
            var result = new GetMemberResult();

            var member = await _context.Members.Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.Gender)
                .Include(a => a.Region).ThenInclude(b => b.Country)
                .Include(a => a.PreferredFiatCurrency)
                .Include(a => a.PreferredCoinCurrency)
                .Include(a => a.PreferredTimeFrame)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            return _mapper.Map<GetMemberResult>(member);
        }
    }
}
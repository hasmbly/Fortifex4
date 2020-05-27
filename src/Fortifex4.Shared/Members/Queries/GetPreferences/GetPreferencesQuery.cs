using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Queries.GetPreferences
{
    public class GetPreferencesQuery : IRequest<GetPreferencesResult>
    {
        public string MemberUsername { get; set; }
    }

    public class GetPreferencesQueryHandler : IRequestHandler<GetPreferencesQuery, GetPreferencesResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetPreferencesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetPreferencesResult> Handle(GetPreferencesQuery query, CancellationToken cancellationToken)
        {
            var result = new GetPreferencesResult();

            var member = await _context.Members
                .Where(x => x.MemberUsername == query.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            result.PreferredCoinCurrencyID = member.PreferredCoinCurrencyID;
            result.PreferredFiatCurrencyID = member.PreferredFiatCurrencyID;
            result.PreferredTimeFrameID = member.PreferredTimeFrameID;

            return result;
        }
    }
}
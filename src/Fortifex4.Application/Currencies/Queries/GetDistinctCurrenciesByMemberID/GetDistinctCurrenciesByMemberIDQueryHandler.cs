using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDQueryHandler : IRequestHandler<GetDistinctCurrenciesByMemberIDQuery, GetDistinctCurrenciesByMemberIDResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetDistinctCurrenciesByMemberIDQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDistinctCurrenciesByMemberIDResult> Handle(GetDistinctCurrenciesByMemberIDQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _context.Pockets
                .Where(x => x.Wallet.Owner.MemberUsername == request.MemberUsername && x.CurrencyType == CurrencyType.Coin)
                .Include(x => x.Currency)
                .Select(x => x.Currency).Distinct()
                .ProjectTo<CurrencyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new GetDistinctCurrenciesByMemberIDResult { Currencies = currencies };
        }
    }
}
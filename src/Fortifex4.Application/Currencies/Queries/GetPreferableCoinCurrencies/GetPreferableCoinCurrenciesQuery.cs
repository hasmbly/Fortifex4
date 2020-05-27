using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesQuery : IRequest<GetPreferableCoinCurrenciesResult>
    {
    }

    public class GetPreferableCoinCurrenciesQueryHandler : IRequestHandler<GetPreferableCoinCurrenciesQuery, GetPreferableCoinCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetPreferableCoinCurrenciesQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetPreferableCoinCurrenciesResult> Handle(GetPreferableCoinCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var coinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin && x.IsForPreferredOption)
                .OrderBy(x => x.Name)
                .ProjectTo<CoinCurrencyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new GetPreferableCoinCurrenciesResult { CoinCurrencies = coinCurrencies };
        }
    }
}
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetAllCoinCurrencies
{
    public class GetAllCoinCurrenciesQuery : IRequest<GetAllCoinCurrenciesResult>
    {
    }

    public class GetAllCoinCurrenciesQueryHandler : IRequestHandler<GetAllCoinCurrenciesQuery, GetAllCoinCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetAllCoinCurrenciesQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllCoinCurrenciesResult> Handle(GetAllCoinCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var coinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin)
                .Include(x => x.Blockchain)
                .OrderBy(x => x.Rank)
                .ProjectTo<CoinCurrencyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new GetAllCoinCurrenciesResult { CoinCurrencies = coinCurrencies };
        }
    }
}
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

        public GetAllCoinCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllCoinCurrenciesResult> Handle(GetAllCoinCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllCoinCurrenciesResult();

            var coinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin)
                .Include(x => x.Blockchain)
                .OrderBy(x => x.Rank)
                .ToListAsync(cancellationToken);

            foreach (var coinCurrency in coinCurrencies)
            {
                result.CoinCurrencies.Add(new CoinCurrencyDTO
                {
                    CurrencyID = coinCurrency.CurrencyID,
                    BlockchainName = coinCurrency.Blockchain.Name,
                    Name = coinCurrency.Name,
                    Symbol = coinCurrency.Symbol
                });
            }

            return result;
        }
    }
}
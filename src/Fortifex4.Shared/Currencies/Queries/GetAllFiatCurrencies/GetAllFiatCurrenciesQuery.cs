using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetAllFiatCurrencies
{
    public class GetAllFiatCurrenciesQuery : IRequest<GetAllFiatCurrenciesResult>
    {
    }

    public class GetAllFiatCurrenciesQueryHandler : IRequestHandler<GetAllFiatCurrenciesQuery, GetAllFiatCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllFiatCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllFiatCurrenciesResult> Handle(GetAllFiatCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllFiatCurrenciesResult();

            var fiatCurrencies = await _context.Currencies
                .Where(x =>
                    x.CurrencyType == CurrencyType.Fiat &&
                    x.UnitPriceInUSD > 0)
                .OrderBy(x => x.Symbol)
                .ToListAsync(cancellationToken);

            foreach (var fiatCurrency in fiatCurrencies)
            {
                result.FiatCurrencies.Add(new FiatCurrencyDTO
                {
                    CurrencyID = fiatCurrency.CurrencyID,
                    Symbol = fiatCurrency.Symbol,
                    Name = fiatCurrency.Name,
                    IsShownInTradePair = fiatCurrency.IsShownInTradePair,
                    IsForPreferredOption = fiatCurrency.IsForPreferredOption,
                    UnitPriceInUSD = fiatCurrency.UnitPriceInUSD
                });
            }

            return result;
        }
    }
}
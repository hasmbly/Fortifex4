using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Currencies.Queries.GetAllFiatCurrencies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetAllFiatCurrencies
{
    public class GetAllFiatCurrenciesQueryHandler : IRequestHandler<GetAllFiatCurrenciesRequest, GetAllFiatCurrenciesResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllFiatCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllFiatCurrenciesResponse> Handle(GetAllFiatCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllFiatCurrenciesResponse();

            var fiatCurrencies = await _context.Currencies
                .Where(x =>
                    x.CurrencyType == CurrencyType.Fiat &&
                    x.UnitPriceInUSD > 0)
                .OrderBy(x => x.Symbol)
                .ToListAsync(cancellationToken);

            if (fiatCurrencies.Count == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.FiatCurrenciesNotFound;

                return result;
            }

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

            result.IsSuccessful = true;

            return result;
        }
    }
}
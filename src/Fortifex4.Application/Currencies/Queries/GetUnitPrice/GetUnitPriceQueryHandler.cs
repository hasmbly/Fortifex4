using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Currencies.Queries.GetUnitPrice;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetUnitPrice
{
    public class GetUnitPriceQueryHandler : IRequestHandler<GetUnitPriceRequest, GetUnitPriceResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetUnitPriceQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetUnitPriceResponse> Handle(GetUnitPriceRequest request, CancellationToken cancellationToken)
        {
            var result = new GetUnitPriceResponse();

            if (request.FromCurrencySymbol != request.ToCurrencySymbol)
            {
                var fromCurrency = await _context.Currencies
                    .Where(x => x.Symbol == request.FromCurrencySymbol)
                    .OrderBy(x => x.Rank)
                    .FirstOrDefaultAsync(cancellationToken);

                if (fromCurrency != null)
                {
                    if (request.ToCurrencySymbol == CurrencySymbol.USD)
                    {
                        result.UnitPrice = fromCurrency.UnitPriceInUSD;
                    }
                    else
                    {
                        var toCurrency = await _context.Currencies
                            .Where(x => x.Symbol == request.ToCurrencySymbol)
                            .OrderBy(x => x.Rank)
                            .FirstOrDefaultAsync(cancellationToken);

                        if (toCurrency != null)
                        {
                            if (toCurrency.UnitPriceInUSD > 0)
                            {
                                // Misalnya ETH ke GBP
                                // 1 ETH = 1000 USD
                                // 1 GBP = 2 USD
                                // 1 ETH = 500 GBP
                                result.UnitPrice = fromCurrency.UnitPriceInUSD / toCurrency.UnitPriceInUSD;
                            }
                        }
                    } 
                }
                else
                {
                    result.IsSuccessful = false;
                    result.ErrorMessage = ErrorMessage.CurrencyNotFound;

                    return result;
                }
            }
            else
            {
                result.UnitPrice = 1m;
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
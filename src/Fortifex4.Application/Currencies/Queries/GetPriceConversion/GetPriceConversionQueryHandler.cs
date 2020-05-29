using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Currencies.Queries.GetPriceConversion;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetPriceConversion
{
    public class GetPriceConversionQueryHandler : IRequestHandler<GetPriceConversionRequest, GetPriceConversionResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetPriceConversionQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetPriceConversionResponse> Handle(GetPriceConversionRequest request, CancellationToken cancellationToken)
        {
            var result = new GetPriceConversionResponse();

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
                        result.ConvertedAmount = request.Amount * fromCurrency.UnitPriceInUSD;
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
                                result.ConvertedAmount = request.Amount * (fromCurrency.UnitPriceInUSD / toCurrency.UnitPriceInUSD);
                            }
                        }
                    }
                }
            }
            else
            {
                result.ConvertedAmount = request.Amount;
            }

            return result;
        }
    }
}
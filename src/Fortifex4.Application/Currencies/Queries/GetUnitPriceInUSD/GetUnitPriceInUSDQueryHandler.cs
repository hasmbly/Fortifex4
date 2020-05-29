using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Currencies.Queries.GetUnitPriceInUSD;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetUnitPriceInUSD
{
    public class GetUnitPriceInUSDQueryHandler : IRequestHandler<GetUnitPriceInUSDRequest, GetUnitPriceInUSDResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetUnitPriceInUSDQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetUnitPriceInUSDResponse> Handle(GetUnitPriceInUSDRequest request, CancellationToken cancellationToken)
        {
            var result = new GetUnitPriceInUSDResponse();

            var currency = await _context.Currencies
                .Where(x => x.Symbol == request.CurrencySymbol)
                .OrderBy(x => x.Rank)
                .FirstOrDefaultAsync(cancellationToken);

            if (currency != null)
                result.UnitPriceInUSD = currency.UnitPriceInUSD;

            return result;
        }
    }
}
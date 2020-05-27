using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetUnitPriceInUSD
{
    public class GetUnitPriceInUSDQuery : IRequest<decimal>
    {
        public string CurrencySymbol { get; set; }
    }

    public class GetUnitPriceInUSDQueryHandler : IRequestHandler<GetUnitPriceInUSDQuery, decimal>
    {
        private readonly IFortifex4DBContext _context;

        public GetUnitPriceInUSDQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<decimal> Handle(GetUnitPriceInUSDQuery request, CancellationToken cancellationToken)
        {
            decimal unitPriceInUSD = 0m;

            var currency = await _context.Currencies
                .Where(x => x.Symbol == request.CurrencySymbol)
                .OrderBy(x => x.Rank)
                .FirstOrDefaultAsync(cancellationToken);

            if (currency != null)
                unitPriceInUSD = currency.UnitPriceInUSD;

            return unitPriceInUSD;
        }
    }
}
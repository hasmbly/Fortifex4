using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Currencies.Queries.GetCurrency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetCurrency
{
    public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyRequest, GetCurrencyResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetCurrencyQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetCurrencyResponse> Handle(GetCurrencyRequest query, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == query.CurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (currency == null)
                throw new NotFoundException(nameof(Currency), query.CurrencyID);

            return new GetCurrencyResponse
            {
                CurrencyID = currency.CurrencyID,
                Symbol = currency.Symbol,
                Name = currency.Name
            };
        }
    }
}
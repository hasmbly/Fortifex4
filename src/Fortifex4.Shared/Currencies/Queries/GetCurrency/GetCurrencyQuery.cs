using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetCurrency
{
    public class GetCurrencyQuery : IRequest<GetCurrencyResult>
    {
        public int CurrencyID { get; set; }
    }

    public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, GetCurrencyResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetCurrencyQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetCurrencyResult> Handle(GetCurrencyQuery query, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == query.CurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (currency == null)
                throw new NotFoundException(nameof(Currency), query.CurrencyID);

            return new GetCurrencyResult
            {
                CurrencyID = currency.CurrencyID,
                Symbol = currency.Symbol,
                Name = currency.Name
            };
        }
    }
}
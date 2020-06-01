using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
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
            var result = new GetCurrencyResponse();

            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == query.CurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (currency == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.CurrencyNotFound;

                return result;
            }

            result.IsSuccessful = true;
            result.CurrencyID = currency.CurrencyID;
            result.Symbol = currency.Symbol;
            result.Name = currency.Name;

            return result;
        }
    }
}
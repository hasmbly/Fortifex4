using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Currencies.Queries.GetDistinctCurrenciesByMemberID;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetDistinctCurrenciesByMemberID
{
    public class GetDistinctCurrenciesByMemberIDQueryHandler : IRequestHandler<GetDistinctCurrenciesByMemberIDRequest, GetDistinctCurrenciesByMemberIDResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetDistinctCurrenciesByMemberIDQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetDistinctCurrenciesByMemberIDResponse> Handle(GetDistinctCurrenciesByMemberIDRequest request, CancellationToken cancellationToken)
        {
            var result = new GetDistinctCurrenciesByMemberIDResponse();

            var currencies = await _context.Pockets
                .Where(x => x.Wallet.Owner.MemberUsername == request.MemberUsername && x.CurrencyType == CurrencyType.Coin)
                .Include(x => x.Currency)
                .Select(x => x.Currency).Distinct()
                .ToListAsync(cancellationToken);

            foreach (var currency in currencies)
            {
                result.Currencies.Add(new CurrencyDTO
                {
                    CurrencyID = currency.CurrencyID,
                    Symbol = currency.Symbol,
                    Name = currency.Name
                });
            }

            return result;
        }
    }
}
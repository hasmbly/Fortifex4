using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Currencies.Queries.GetPreferrableCoinCurrencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetPreferrableCoinCurrencies
{
    public class GetPreferableCoinCurrenciesQueryHandler : IRequestHandler<GetPreferableCoinCurrenciesRequest, GetPreferableCoinCurrenciesResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetPreferableCoinCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetPreferableCoinCurrenciesResponse> Handle(GetPreferableCoinCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetPreferableCoinCurrenciesResponse();

            var coinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin && x.IsForPreferredOption)
                .OrderBy(x => x.Name)
                .Include(a => a.Blockchain)
                .ToListAsync(cancellationToken);

            foreach (var coinCurrency in coinCurrencies)
            {
                result.CoinCurrencies.Add(new CoinCurrencyDTO 
                { 
                    CurrencyID = coinCurrency.CurrencyID,
                    BlockchainID = coinCurrency.Blockchain.BlockchainID,
                    Symbol = coinCurrency.Symbol,
                    Name = coinCurrency.Name,
                    IsForPreferredOption = coinCurrency.IsForPreferredOption,
                    IsShownInTradePair = coinCurrency.IsShownInTradePair
                });
            }

            return result;
        }
    }
}
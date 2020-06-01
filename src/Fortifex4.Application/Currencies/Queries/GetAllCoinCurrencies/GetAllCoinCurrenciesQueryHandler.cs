using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Currencies.Queries.GetAllCoinCurrencies;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Currencies.Queries.GetAllCoinCurrencies
{
    public class GetAllCoinCurrenciesQueryHandler : IRequestHandler<GetAllCoinCurrenciesRequest, GetAllCoinCurrenciesResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllCoinCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllCoinCurrenciesResponse> Handle(GetAllCoinCurrenciesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllCoinCurrenciesResponse();

            var coinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin)
                .Include(x => x.Blockchain)
                .OrderBy(x => x.Rank)
                .ToListAsync(cancellationToken);

            if (coinCurrencies.Count == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.CoinCurrenciesNotFound;

                return result;
            }

            foreach (var coinCurrency in coinCurrencies)
            {
                result.CoinCurrencies.Add(new CoinCurrencyDTO
                {
                    CurrencyID = coinCurrency.CurrencyID,
                    BlockchainName = coinCurrency.Blockchain.Name,
                    Name = coinCurrency.Name,
                    Symbol = coinCurrency.Symbol
                });
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
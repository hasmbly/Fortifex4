using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Currencies.Queries.GetDestinationCurrenciesForMember;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberQueryHandler : IRequestHandler<GetDestinationCurrenciesForMemberRequest, GetDestinationCurrenciesForMemberResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetDestinationCurrenciesForMemberQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetDestinationCurrenciesForMemberResponse> Handle(GetDestinationCurrenciesForMemberRequest query, CancellationToken cancellationToken)
        {
            var result = new GetDestinationCurrenciesForMemberResponse();

            var member = await _context.Members
                .Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.PreferredFiatCurrency).ThenInclude(b => b.Blockchain)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
            {
                result.IsSuccessful = true;
                result.ErrorMessage = ErrorMessage.MemberUsernameNotFound;

                return result;
            }

            if (member.PreferredFiatCurrency.Symbol != CurrencySymbol.USD)
            {
                result.Currencies.Add(new CurrencyDTO 
                {
                    CurrencyID = member.PreferredFiatCurrency.CurrencyID,
                    Name = member.PreferredFiatCurrency.Name,
                    Symbol = member.PreferredFiatCurrency.Symbol,
                    CurrencyType = member.PreferredFiatCurrency.CurrencyType,
                    BlockchainName = member.PreferredFiatCurrency.Blockchain.Name
                });
            }

            var currencyUSD = await _context.Currencies
                .Where(x => x.Symbol == CurrencySymbol.USD)
                .Include(a => a.Blockchain)
                .SingleOrDefaultAsync(cancellationToken);

            result.Currencies.Add(new CurrencyDTO
            {
                CurrencyID = currencyUSD.CurrencyID,
                Name = currencyUSD.Name,
                Symbol = currencyUSD.Symbol,
                CurrencyType = currencyUSD.CurrencyType,
                BlockchainName = currencyUSD.Blockchain.Name
            });            

            var availableCoinCurrenciesForPreferredOptions = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin && x.IsForPreferredOption)
                .OrderBy(x => x.Name)
                .Include(a => a.Blockchain)
                .ToListAsync(cancellationToken);

            foreach (var availableCoinCurrencyForPreferredOptions in availableCoinCurrenciesForPreferredOptions)
            {
                result.Currencies.Add(new CurrencyDTO
                {
                    CurrencyID = availableCoinCurrencyForPreferredOptions.CurrencyID,
                    Name = availableCoinCurrencyForPreferredOptions.Name,
                    Symbol = availableCoinCurrencyForPreferredOptions.Symbol,
                    CurrencyType = availableCoinCurrencyForPreferredOptions.CurrencyType,
                    BlockchainName = availableCoinCurrencyForPreferredOptions.Blockchain.Name
                });
            }

            result.IsSuccessful = true;
            
            return result;
        }
    }
}
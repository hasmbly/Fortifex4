using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetDestinationCurrenciesForMember
{
    public class GetDestinationCurrenciesForMemberQueryHandler : IRequestHandler<GetDestinationCurrenciesForMemberQuery, GetDestinationCurrenciesForMemberResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetDestinationCurrenciesForMemberQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDestinationCurrenciesForMemberResult> Handle(GetDestinationCurrenciesForMemberQuery query, CancellationToken cancellationToken)
        {
            List<CurrencyDTO> destinationCurrencies = new List<CurrencyDTO>();

            var member = await _context.Members
                .Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.PreferredFiatCurrency)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            if (member.PreferredFiatCurrency.Symbol != CurrencySymbol.USD)
            {
                destinationCurrencies.Add(_mapper.Map<CurrencyDTO>(member.PreferredFiatCurrency));
            }

            var currencyUSD = await _context.Currencies
                .Where(x => x.Symbol == CurrencySymbol.USD)
                .SingleOrDefaultAsync(cancellationToken);

            destinationCurrencies.Add(_mapper.Map<CurrencyDTO>(currencyUSD));

            var availableCoinCurrenciesForPreferredOptions = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin && x.IsForPreferredOption)
                .OrderBy(x => x.Name)
                .ProjectTo<CurrencyDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            destinationCurrencies.AddRange(availableCoinCurrenciesForPreferredOptions);

            return new GetDestinationCurrenciesForMemberResult { Currencies = destinationCurrencies };
        }
    }
}
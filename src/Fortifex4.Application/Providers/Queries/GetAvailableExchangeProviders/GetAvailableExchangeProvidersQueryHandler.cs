using AutoMapper;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersQueryHandler : IRequestHandler<GetAvailableExchangeProvidersQuery, GetAvailableExchangeProvidersResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetAvailableExchangeProvidersQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAvailableExchangeProvidersResult> Handle(GetAvailableExchangeProvidersQuery query, CancellationToken cancellationToken)
        {
            var exchangeProviders = await _context.Providers
                .Where(x => x.ProviderType == ProviderType.Exchange)
                .ToListAsync(cancellationToken);

            var memberExchangeOwners = await _context.Owners
                .Where(x => x.MemberUsername == query.MemberUsername && x.ProviderType == ProviderType.Exchange)
                .ToListAsync(cancellationToken);

            List<ExchangeProviderDTO> availableExchangeProviders = new List<ExchangeProviderDTO>();

            foreach (var exchangeProvider in exchangeProviders)
            {
                if (!memberExchangeOwners.Any(x => x.ProviderID == exchangeProvider.ProviderID))
                {
                    availableExchangeProviders.Add(_mapper.Map<ExchangeProviderDTO>(exchangeProvider));
                }
            }

            var result = new GetAvailableExchangeProvidersResult
            {
                ExchangeProviders = availableExchangeProviders
            };

            return result;
        }
    }
}
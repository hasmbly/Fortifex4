using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersQueryHandler : IRequestHandler<GetAvailableExchangeProvidersRequest, GetAvailableExchangeProvidersResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAvailableExchangeProvidersQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAvailableExchangeProvidersResponse> Handle(GetAvailableExchangeProvidersRequest query, CancellationToken cancellationToken)
        {
            var result = new GetAvailableExchangeProvidersResponse();

            var exchangeProviders = await _context.Providers
                .Where(x => x.ProviderType == ProviderType.Exchange)
                .ToListAsync(cancellationToken);

            var memberExchangeOwners = await _context.Owners
                .Where(x => x.MemberUsername == query.MemberUsername && x.ProviderType == ProviderType.Exchange)
                .ToListAsync(cancellationToken);

            foreach (var exchangeProvider in exchangeProviders)
            {
                if (!memberExchangeOwners.Any(x => x.ProviderID == exchangeProvider.ProviderID))
                {
                    //availableExchangeProviders.Add(_mapper.Map<ExchangeProviderDTO>(exchangeProvider));
                    result.ExchangeProviders.Add(new ExchangeProviderDTO
                    {
                        Name = exchangeProvider.Name,
                        ProviderID = exchangeProvider.ProviderID,
                        SiteURL = exchangeProvider.SiteURL
                    });
                }
            }      

            return result;
        }
    }
}
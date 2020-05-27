using MediatR;

namespace Fortifex4.Application.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersQuery : IRequest<GetAvailableExchangeProvidersResult>
    {
        public string MemberUsername { get; set; }
    }
}
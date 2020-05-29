using MediatR;

namespace Fortifex4.Shared.Providers.Queries.GetAvailableExchangeProviders
{
    public class GetAvailableExchangeProvidersRequest : IRequest<GetAvailableExchangeProvidersResponse>
    {
        public string MemberUsername { get; set; }
    }
}
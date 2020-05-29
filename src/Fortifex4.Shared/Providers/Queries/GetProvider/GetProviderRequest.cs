using MediatR;

namespace Fortifex4.Shared.Providers.Queries.GetProvider
{
    public class GetProviderRequest : IRequest<GetProviderResponse>
    {
        public int ProviderID { get; set; }
    }
}
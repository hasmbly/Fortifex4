using MediatR;

namespace Fortifex4.Application.Providers.Queries.GetProvider
{
    public class GetProviderQuery : IRequest<GetProviderResult>
    {
        public int ProviderID { get; set; }
    }
}
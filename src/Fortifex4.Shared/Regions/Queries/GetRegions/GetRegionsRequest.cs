using MediatR;

namespace Fortifex4.Shared.Regions.Queries.GetRegions
{
    public class GetRegionsRequest : IRequest<GetRegionsResponse>
    {
        public string CountryCode { get; set; }
    }
}
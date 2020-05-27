using MediatR;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQuery : IRequest<GetRegionsResult>
    {
        public string CountryCode { get; set; }
    }
}
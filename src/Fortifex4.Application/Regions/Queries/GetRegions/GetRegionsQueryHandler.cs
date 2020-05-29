using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Regions.Queries.GetRegions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsRequest, GetRegionsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetRegionsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetRegionsResponse> Handle(GetRegionsRequest request, CancellationToken cancellationToken)
        {
            var result = new GetRegionsResponse();

            var regions = await _context.Regions
                .Where(x => x.CountryCode == request.CountryCode)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            if (regions.Count > 0)
            {
                foreach (var region in regions)
                {
                    result.Regions.Add(new RegionDTO
                    {
                        RegionID = region.RegionID,
                        CountryCode = region.CountryCode,
                        Name = region.Name
                    });
                }
            }
            else
            {
                var undefinedRegions = await _context.Regions
                    .Where(x => x.CountryCode == CountryCode.Undefined)
                    .OrderBy(x => x.Name)
                    .ToListAsync(cancellationToken);

                foreach (var undefinedRegion in undefinedRegions)
                {
                    result.Regions.Add(new RegionDTO
                    {
                        RegionID = undefinedRegion.RegionID,
                        CountryCode = undefinedRegion.CountryCode,
                        Name = undefinedRegion.Name
                    });
                }
            }

            return result;
        }
    }
}
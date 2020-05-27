using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetRegionsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetRegionsResult> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            GetRegionsResult result = new GetRegionsResult();

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
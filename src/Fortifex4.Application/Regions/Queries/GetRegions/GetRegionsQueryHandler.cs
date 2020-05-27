using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, GetRegionsResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetRegionsQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetRegionsResult> Handle(GetRegionsQuery request, CancellationToken cancellationToken)
        {
            GetRegionsResult result = new GetRegionsResult();

            var regions = await _context.Regions
                .Where(x => x.CountryCode == request.CountryCode)
                .OrderBy(x => x.Name)
                .ProjectTo<RegionDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (regions.Count > 0)
            {
                result.Regions = regions;
            }
            else
            {
                var undefinedRegions = await _context.Regions
                    .Where(x => x.CountryCode == CountryCode.Undefined)
                    .OrderBy(x => x.Name)
                    .ProjectTo<RegionDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                result.Regions = undefinedRegions;
            }

            return result;
        }
    }
}
using System.Collections.Generic;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsResult
    {
        public IEnumerable<RegionDTO> Regions { get; set; }
    }
}
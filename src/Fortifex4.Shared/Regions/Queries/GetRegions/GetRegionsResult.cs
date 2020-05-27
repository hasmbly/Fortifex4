using System.Collections.Generic;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class GetRegionsResult
    {
        public IList<RegionDTO> Regions { get; set; }

        public GetRegionsResult()
        {
            this.Regions = new List<RegionDTO>();
        }
    }
}
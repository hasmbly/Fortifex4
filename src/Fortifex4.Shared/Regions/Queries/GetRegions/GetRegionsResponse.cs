using System.Collections.Generic;

namespace Fortifex4.Shared.Regions.Queries.GetRegions
{
    public class GetRegionsResponse
    {
        public IList<RegionDTO> Regions { get; set; }

        public GetRegionsResponse()
        {
            this.Regions = new List<RegionDTO>();
        }
    }
}
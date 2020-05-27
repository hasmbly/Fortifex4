using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class RegionDTO
    {
        public int RegionID { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}
using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Regions.Queries.GetRegions
{
    public class RegionDTO : IMapFrom<Region>
    {
        public int RegionID { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Region, RegionDTO>();
        }
    }
}
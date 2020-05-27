using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class TimeFrameDTO : IMapFrom<TimeFrame>
    {
        public int TimeFrameID { get; set; }
        public string Name { get; set; }
        public int TimeSpanInHours { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TimeFrame, TimeFrameDTO>();
        }
    }
}
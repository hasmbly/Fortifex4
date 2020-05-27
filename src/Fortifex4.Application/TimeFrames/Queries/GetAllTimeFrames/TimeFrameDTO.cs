using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class TimeFrameDTO
    {
        public int TimeFrameID { get; set; }
        public string Name { get; set; }
        public int TimeSpanInHours { get; set; }
    }
}
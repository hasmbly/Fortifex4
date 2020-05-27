using System.Collections.Generic;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class GetAllTimeFramesResult
    {
        public IEnumerable<TimeFrameDTO> TimeFrames { get; set; }
    }
}
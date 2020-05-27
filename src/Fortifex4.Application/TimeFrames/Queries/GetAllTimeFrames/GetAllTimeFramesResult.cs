using System.Collections.Generic;
using System.Data;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class GetAllTimeFramesResult
    {
        public IList<TimeFrameDTO> TimeFrames { get; set; }

        public GetAllTimeFramesResult()
        {
            this.TimeFrames = new List<TimeFrameDTO>();
        }
    }
}
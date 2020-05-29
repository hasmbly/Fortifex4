using System.Collections.Generic;

namespace Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames
{
    public class GetAllTimeFramesResponse
    {
        public IList<TimeFrameDTO> TimeFrames { get; set; }

        public GetAllTimeFramesResponse()
        {
            this.TimeFrames = new List<TimeFrameDTO>();
        }
    }
}
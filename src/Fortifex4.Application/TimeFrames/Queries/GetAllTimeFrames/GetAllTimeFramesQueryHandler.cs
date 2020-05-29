using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.TimeFrames.Queries.GetAllTimeFrames;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class GetAllTimeFramesQueryHandler : IRequestHandler<GetAllTimeFramesRequest, GetAllTimeFramesResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllTimeFramesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllTimeFramesResponse> Handle(GetAllTimeFramesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllTimeFramesResponse();

            var timeFrames = await _context.TimeFrames.ToListAsync(cancellationToken);

            foreach (var timeFrame in timeFrames)
            {
                result.TimeFrames.Add(new TimeFrameDTO
                {
                    TimeFrameID = timeFrame.TimeFrameID,
                    Name = timeFrame.Name,
                    TimeSpanInHours = timeFrame.TimeFrameID
                });
            }

            return result;
        }
    }
}
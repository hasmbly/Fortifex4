using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.TimeFrames.Queries.GetAllTimeFrames
{
    public class GetAllTimeFramesQueryHandler : IRequestHandler<GetAllTimeFramesQuery, GetAllTimeFramesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetAllTimeFramesQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllTimeFramesResult> Handle(GetAllTimeFramesQuery request, CancellationToken cancellationToken)
        {
            var timeFrames = await _context.TimeFrames.ProjectTo<TimeFrameDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new GetAllTimeFramesResult { TimeFrames = timeFrames };
        }
    }
}
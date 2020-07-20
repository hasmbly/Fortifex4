using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Fortifex4.Application.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID
{
    public class GetProjectStatusLogsByProjectIDQueryHandler : IRequestHandler<GetProjectStatusLogsByProjectIDRequest, GetProjectStatusLogsByProjectIDResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetProjectStatusLogsByProjectIDQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetProjectStatusLogsByProjectIDResponse> Handle(GetProjectStatusLogsByProjectIDRequest request, CancellationToken cancellationToken)
        {
            var result = new GetProjectStatusLogsByProjectIDResponse();

            var projectStatusLogs = await _context.ProjectStatusLogs
                .Where(x => x.ProjectID == request.ProjectID)
                .ToListAsync(cancellationToken);

            foreach (var projectStatusLog in projectStatusLogs)
            {
                ProjectStatusLogDTO projectStatusLogsDTO = new ProjectStatusLogDTO
                {
                    LastModified = projectStatusLog.LastModified,
                    ProjectStatus = projectStatusLog.ProjectStatus,
                    Comment = projectStatusLog.Comment
                };

                result.ProjectStatusLogs.Add(projectStatusLogsDTO);
            }

            return result;
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Projects.Commands.UpdateProjectStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Commands.UpdateProjectStatus
{
    public class UpdateProjectStatusQueryHandler : IRequestHandler<UpdateProjectStatusRequest, UpdateProjectStatusResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateProjectStatusQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateProjectStatusResponse> Handle(UpdateProjectStatusRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateProjectStatusResponse();

            var project = await _context.Projects
                .Where(x => x.ProjectID == request.ProjectID)
                .SingleOrDefaultAsync(cancellationToken);

            if (project == null)
                throw new NotFoundException(nameof(Project), request.ProjectID);

            project.ProjectStatus = request.NewProjectStatus;

            await _context.SaveChangesAsync(cancellationToken);

            var projectStatusLog = new ProjectStatusLog
            {
                ProjectID = project.ProjectID,
                ProjectStatus = project.ProjectStatus,
                Comment = request.Comment
            };

            await _context.ProjectStatusLogs.AddAsync(projectStatusLog);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;
            result.ProjectID = project.ProjectID;
            result.NewProjectStatus = request.NewProjectStatus;

            return result;
        }
    }
}
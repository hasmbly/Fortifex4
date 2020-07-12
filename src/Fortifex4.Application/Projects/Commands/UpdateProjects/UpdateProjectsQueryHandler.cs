using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Projects.Commands.UpdateProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Commands.UpdateProjects
{
    public class UpdateProjectsQueryHandler : IRequestHandler<UpdateProjectsRequest, UpdateProjectsResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public UpdateProjectsQueryHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<UpdateProjectsResponse> Handle(UpdateProjectsRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateProjectsResponse();

            var project = await _context.Projects
                .Where(x => x.ProjectID == request.ProjectID)
                .SingleOrDefaultAsync(cancellationToken);

            if (project != null)
            {
                project.BlockchainID = request.ProjectBlockchainID;
                project.Name = request.ProjectName;
                project.Description = request.ProjectDescription;
                project.WalletAddress = request.ProjectWalletAddress;
                project.LastModified = _dateTimeOffset.Now;

                await _context.SaveChangesAsync(cancellationToken);

                result.IsSuccessful = true;
                result.ProjectID = project.ProjectID;
            }

            return result;
        }
    }
}
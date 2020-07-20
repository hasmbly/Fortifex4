using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Projects.Commands.CreateProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Commands.CreateProjects
{
    public class CreateProjectsCommandHandler : IRequestHandler<CreateProjectsRequest, CreateProjectsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public CreateProjectsCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CreateProjectsResponse> Handle(CreateProjectsRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateProjectsResponse()
            {
                IsSuccessful = false
            };

            #region Check if Member already have Project
            var member = await _context.Projects
                .Where(x => x.MemberUsername == request.MemberUsername)
                .FirstOrDefaultAsync(cancellationToken);

            if (member != null)
            {
                result.ProjectID = member.ProjectID;
                return result;
            }
            #endregion

            #region Add New Project
            var project = new Project
            {
                BlockchainID = request.BlockchainID,
                MemberUsername = request.MemberUsername,
                Name = request.Name,
                Description = request.Description,
                WalletAddress = request.WalletAddress,
                ProjectStatus = ProjectStatus.Draft
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            var projectStatusLog = new ProjectStatusLog
            {
                ProjectID = project.ProjectID,
                ProjectStatus = ProjectStatus.Draft,
                Comment = "Project has been created"
            };

            await _context.ProjectStatusLogs.AddAsync(projectStatusLog);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;
            result.ProjectID = project.ProjectID;

            return result;
        }
    }
}
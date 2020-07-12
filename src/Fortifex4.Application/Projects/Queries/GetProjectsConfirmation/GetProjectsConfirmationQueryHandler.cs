using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Projects.Queries.GetProjectsConfirmation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Queries.GetProjectsConfirmation
{
    public class GetProjectApprovalQueryHandler : IRequestHandler<GetProjectsConfirmationRequest, GetProjectsConfirmationResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetProjectApprovalQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetProjectsConfirmationResponse> Handle(GetProjectsConfirmationRequest query, CancellationToken cancellationToken)
        {
            var result = new GetProjectsConfirmationResponse();

            var projects = await _context.Projects
                .Where(x => x.ProjectStatus == ProjectStatus.SubmittedForApproval)
                .Include(a => a.Blockchain)
                .ToListAsync(cancellationToken);

            foreach (var project in projects)
            {
                ProjectDTO projectDTO = new ProjectDTO
                {
                    ProjectID = project.ProjectID,
                    Name = project.Name,
                    BlockchainName = project.Blockchain.Name
                };

                result.Projects.Add(projectDTO);
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
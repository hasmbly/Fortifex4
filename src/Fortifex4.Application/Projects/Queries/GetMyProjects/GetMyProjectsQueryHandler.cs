using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Projects.Queries.GetMyProjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsQueryHandler : IRequestHandler<GetMyProjectsRequest, GetMyProjectsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetMyProjectsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetMyProjectsResponse> Handle(GetMyProjectsRequest query, CancellationToken cancellationToken)
        {
            var result = new GetMyProjectsResponse();

            var projects = await _context.Projects
                .Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.Blockchain)
                .Include(a => a.Contributors)
                .ToListAsync(cancellationToken);

            foreach (var project in projects)
            {
                ProjectDTO projectDTO = new ProjectDTO
                {
                    ProjectID = project.ProjectID,
                    MemberUsername = project.MemberUsername,
                    Name = project.Name,
                    BlockchainID = project.Blockchain.BlockchainID,
                    BlockchainName = project.Blockchain.Name
                };

                result.Projects.Add(projectDTO);
            }

            return result;
        }
    }
}
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsQueryHandler : IRequestHandler<GetMyProjectsQuery, GetMyProjectsResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetMyProjectsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetMyProjectsResult> Handle(GetMyProjectsQuery query, CancellationToken cancellationToken)
        {
            GetMyProjectsResult result = new GetMyProjectsResult();

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
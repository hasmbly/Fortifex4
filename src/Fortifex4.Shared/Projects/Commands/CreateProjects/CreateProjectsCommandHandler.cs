using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Commands.CreateProjects
{
    public class CreateProjectsCommandHandler : IRequestHandler<CreateProjectsCommand, CreateProjectsResult>
    {
        private readonly IFortifex4DBContext _context;

        public CreateProjectsCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CreateProjectsResult> Handle(CreateProjectsCommand request, CancellationToken cancellationToken)
        {
            var result = new CreateProjectsResult()
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
                WalletAddress = request.WalletAddress
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            result.IsSuccessful = true;
            result.ProjectID = project.ProjectID;

            return result;
        }
    }
}
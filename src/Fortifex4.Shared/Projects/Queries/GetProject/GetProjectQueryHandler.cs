using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Queries.GetProject
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, GetProjectResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetProjectQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetProjectResult> Handle(GetProjectQuery query, CancellationToken cancellationToken)
        {
            var result = new GetProjectResult();

            if (query.IsExistProjectByMemberUsername != null)
            {
                result.IsExistProjectByMemberUsernameResult = false;

                var isExistProject = await _context.Projects
                    .Where(x => x.MemberUsername == query.IsExistProjectByMemberUsername)
                    .FirstOrDefaultAsync(cancellationToken);

                if (isExistProject != null)
                {
                    result.IsExistProjectByMemberUsernameResult = true;
                    result.ProjectID = isExistProject.ProjectID;
                }

                return result;
            }

            var project = await _context.Projects
                .Where(x => x.ProjectID == query.ProjectID)
                .Include(a => a.Blockchain)
                .Include(a => a.Contributors)
                .SingleOrDefaultAsync(cancellationToken);

            if (project == null)
                throw new NotFoundException(nameof(Project), query.ProjectID);

            result.ProjectID = query.ProjectID;
            result.MemberUsername = project.MemberUsername;
            result.Name = project.Name;
            result.BlockchainID = project.Blockchain.BlockchainID;
            result.BlockchainName = project.Blockchain.Name;
            result.Description = project.WalletAddress;
            result.WalletAddress = project.WalletAddress;
            result.Contributors = new List<ContributorDTO>();

            foreach (var contributor in project.Contributors)
            {
                var contributorDTO = new ContributorDTO()
                {
                    ContributorID = contributor.ContributorID,
                    MemberUsername = contributor.MemberUsername,
                    InvitationStatus = contributor.InvitationStatus,
                    ProjectID = contributor.ProjectID
                };

                result.Contributors.Add(contributorDTO);
            }

            return result;
        }
    }
}
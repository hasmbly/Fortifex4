using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Contributors.Queries.GetContributorsByMemberUsername;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Contributors.Queries.GetContributorsByMemberUsername
{
    public class GetContributorsByMemberUsernameQueryHandler : IRequestHandler<GetContributorsByMemberUsernameRequest, GetContributorsByMemberUsernameResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetContributorsByMemberUsernameQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetContributorsByMemberUsernameResponse> Handle(GetContributorsByMemberUsernameRequest query, CancellationToken cancellationToken)
        {
            var result = new GetContributorsByMemberUsernameResponse();

            var contributors = await _context.Contributors
                .Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.Project).ThenInclude(a => a.Blockchain)
                .ToListAsync(cancellationToken);

            if (contributors.Count == 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.ContributorNotFound;

                return result;
            }

            foreach (var contributor in contributors)
            {
                ContributorDTO contributorDTO = new ContributorDTO
                {
                    ContributorID = contributor.ContributorID,
                    ProjectID = contributor.ProjectID,
                    InvitationStatus = contributor.InvitationStatus,
                    ProjectName = contributor.Project.Name,
                    ProjectBlockchainName = contributor.Project.Blockchain.Name
                };

                result.Contributors.Add(contributorDTO);
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
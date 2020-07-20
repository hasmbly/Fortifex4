using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Contributors.Queries.CheckIsContributor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Contributors.Queries.CheckIsContributor
{
    public class CheckIsContributorQueryHandler : IRequestHandler<CheckIsContributorRequest, CheckIsContributorResponse>
    {
        private readonly IFortifex4DBContext _context;

        public CheckIsContributorQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CheckIsContributorResponse> Handle(CheckIsContributorRequest request, CancellationToken cancellationToken)
        {
            var result = new CheckIsContributorResponse();

            var contributors = await _context.Contributors.AnyAsync(
                x => x.ProjectID == request.ProjectID &&
                x.MemberUsername == request.MemberUsername &&
                x.InvitationStatus == InvitationStatus.Accepted);

            result.IsContributor = contributors;

            return result;
        }
    }
}
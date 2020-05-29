using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Projects.Queries.GetAllProjectMemberCandidate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateQueryHandler : IRequestHandler<GetAllProjectMemberCandidateRequest, GetAllProjectMemberCandidateResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllProjectMemberCandidateQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllProjectMemberCandidateResponse> Handle(GetAllProjectMemberCandidateRequest query, CancellationToken cancellationToken)
        {
            var members = await _context.Members
                .Where(x => x.MemberUsername != query.ExcludeCreatorUsername)
                .ToListAsync(cancellationToken);

            var result = new GetAllProjectMemberCandidateResponse()
            {
                Members = new List<MembersDTO>()
            };

            foreach (var member in members)
            {
                var memberDTO = new MembersDTO()
                {
                    MemberUsername = member.MemberUsername
                };

                result.Members.Add(memberDTO);
            }

            return result;
        }
    }
}
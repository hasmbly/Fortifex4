using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Projects.Queries.GetAllProjectMemberCandidate
{
    public class GetAllProjectMemberCandidateQueryHandler : IRequestHandler<GetAllProjectMemberCandidateQuery, GetAllProjectMemberCandidateResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllProjectMemberCandidateQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllProjectMemberCandidateResult> Handle(GetAllProjectMemberCandidateQuery query, CancellationToken cancellationToken)
        {
            var members = await _context.Members
                .Where(x => x.MemberUsername != query.ExcludeCreatorUsername)
                .ToListAsync(cancellationToken);

            var result = new GetAllProjectMemberCandidateResult()
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
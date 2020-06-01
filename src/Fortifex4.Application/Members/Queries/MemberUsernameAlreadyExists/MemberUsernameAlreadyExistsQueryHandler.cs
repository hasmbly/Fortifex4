using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Members.Queries.MemberUsernameAlreadyExists;
using MediatR;

namespace Fortifex4.Application.Members.Queries.MemberUsernameAlreadyExists
{
    public class MemberUsernameAlreadyExistsQueryHandler : IRequestHandler<MemberUsernameAlreadyExistsRequest, MemberUsernameAlreadyExistsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public MemberUsernameAlreadyExistsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<MemberUsernameAlreadyExistsResponse> Handle(MemberUsernameAlreadyExistsRequest request, CancellationToken cancellationToken)
        {
            bool doesMemberExist = _context.Members
                .Any(x => x.MemberUsername == request.MemberUsername);

            return new MemberUsernameAlreadyExistsResponse 
            {
                IsSuccessful = true,
                DoesMemberExist = await Task.FromResult(doesMemberExist)
            };
        }
    }
}
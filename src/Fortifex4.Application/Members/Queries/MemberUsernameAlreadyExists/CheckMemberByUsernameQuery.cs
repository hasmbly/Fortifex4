using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR;

namespace Fortifex4.Application.Members.Queries.MemberUsernameAlreadyExists
{
    public class MemberUsernameAlreadyExistsQuery : IRequest<bool>
    {
        public string MemberUsername { get; set; }
    }

    public class MemberUsernameAlreadyExistsQueryHandler : IRequestHandler<MemberUsernameAlreadyExistsQuery, bool>
    {
        private readonly IFortifex4DBContext _context;

        public MemberUsernameAlreadyExistsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(MemberUsernameAlreadyExistsQuery request, CancellationToken cancellationToken)
        {
            bool doesMemberExist = _context.Members
                .Any(x => x.MemberUsername == request.MemberUsername);

            return await Task.FromResult(doesMemberExist);
        }
    }
}
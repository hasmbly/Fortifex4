using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Queries.Login
{
    public class LoginQuery : IRequest<LoginResult>
    {
        public string MemberUsername { get; set; }
        public string Password { get; set; }
    }

    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResult>
    {
        private readonly IFortifex4DBContext _context;

        public LoginQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var result = new LoginResult();

            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member != null)
            {
                result.UsernameExists = true;

                if (member.AuthenticationScheme == SchemeProvider.Fortifex)
                {
                    result.UsingFortifexAuthentication = true;

                    if (FortifexUtility.VerifyPasswordHash(request.Password, member.PasswordSalt, member.PasswordHash))
                    {
                        result.PasswordIsCorrect = true;

                        if (member.ActivationStatus == ActivationStatus.Active)
                        {
                            result.AccountIsActive = true;
                        }
                    }
                }

                var project = await _context.Projects
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

                if (project != null)
                {
                    result.ProjectID = project.ProjectID;
                }
            }

            return result;
        }
    }
}
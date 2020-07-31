using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Members.Common;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Members.Queries.Login;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.Application.Members.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IConfiguration _config;

        public LoginQueryHandler(IFortifex4DBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = new LoginResponse();

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
                            string securityKey = _config.GetSection("Fortifex:TokenSecurityKey").Value;

                            result.AccountIsActive = true;
                            result.IsSuccessful = true;

                            result.Token = TokenHelper.GenerateToken(member, securityKey);
                        }
                        else
                        {
                            result.IsSuccessful = false;
                            result.ErrorMessage = ErrorMessage.InactiveAccount;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = ErrorMessage.InvalidPassword;
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
            else
            {
                result.ErrorMessage = ErrorMessage.MemberUsernameNotFound;
            }

            return result;
        }        
    }
}
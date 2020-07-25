using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Constants;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Members.Queries.Login;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
                            result.AccountIsActive = true;
                            result.IsSuccessful = true;

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Fortifex:TokenSecurityKey").Value));
                            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                            var claims = new[]
                            {
                                new Claim(ClaimTypes.Name, request.MemberUsername),
                                new Claim(ClaimType.PictureUrl, "https://jwt.io/img/pic_logo.svg")
                            };

                            var token = new JwtSecurityToken(
                                null,
                                null,
                                claims,
                                expires: DateTime.Now.AddDays(1),
                                signingCredentials: credentials
                            );

                            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                            result.Token = tokenHandler.WriteToken(token);
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
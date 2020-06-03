using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Email;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Members.Commands.CreateMember;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fortifex4.Application.Members.Commands.CreateMember
{
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberRequest, CreateMemberResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IEmailService _emailService;
        private readonly ICurrentWeb _currentWeb;
        private readonly IConfiguration _config;

        public CreateMemberCommandHandler(IFortifex4DBContext context, IEmailService emailService, ICurrentWeb currentWeb, IConfiguration config)
        {
            _context = context;
            _emailService = emailService;
            _currentWeb = currentWeb;
            _config = config;
        }

        public async Task<CreateMemberResponse> Handle(CreateMemberRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateMemberResponse();

            if (await _context.Members.AnyAsync(x => x.MemberUsername == request.MemberUsername, cancellationToken))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Username already taken";
            }
            else
            {
                var defaultRegion = await _context.Regions
                .Where(x => x.Name == RegionName.Default)
                .SingleOrDefaultAsync(cancellationToken);

                if (defaultRegion == null)
                    throw new NotFoundException(nameof(Region), RegionName.Default);

                var defaultPreferredFiatCurrency = await _context.Currencies
                    .Where(x => x.Symbol == CurrencySymbol.USD && x.CurrencyType == CurrencyType.Fiat)
                    .SingleOrDefaultAsync(cancellationToken);

                if (defaultPreferredFiatCurrency == null)
                    throw new NotFoundException(nameof(Currency), CurrencySymbol.USD);

                var defaultPreferredCoinCurrency = await _context.Currencies
                    .Where(x => x.Symbol == CurrencySymbol.BTC && x.CurrencyType == CurrencyType.Coin)
                    .SingleOrDefaultAsync(cancellationToken);

                if (defaultPreferredCoinCurrency == null)
                    throw new NotFoundException(nameof(Currency), CurrencySymbol.BTC);

                string passwordSalt = FortifexUtility.CreatePasswordSalt();
                string passwordHash = FortifexUtility.CreatePasswordHash(request.Password, passwordSalt);

                var member = new Member
                {
                    MemberUsername = request.MemberUsername,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    AuthenticationScheme = SchemeProvider.Fortifex,
                    RegionID = defaultRegion.RegionID,
                    GenderID = GenderID.Default,
                    BirthDate = MemberBirthDate.Default,
                    PreferredFiatCurrencyID = defaultPreferredFiatCurrency.CurrencyID,
                    PreferredCoinCurrencyID = defaultPreferredCoinCurrency.CurrencyID,
                    PreferredTimeFrameID = TimeFrameID.Default,
                    ActivationCode = Guid.NewGuid(),
                    ActivationStatus = ActivationStatus.Inactive
                };

                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync(cancellationToken);

                result.ActivationCode = member.ActivationCode;

                StringBuilder emailBodyStringBuilder = new StringBuilder();
                emailBodyStringBuilder.AppendLine("Please click below link to activate your Fortifex account.");
                emailBodyStringBuilder.AppendLine("<div>");
                emailBodyStringBuilder.AppendLine($"<a href=\"{_currentWeb.BaseURL}/account/activate?activationCode={member.ActivationCode}\">Activate my Fortifex account</a>");
                emailBodyStringBuilder.AppendLine("</div>");

                EmailItem mailItem = new EmailItem
                {
                    ToAdress = "fuady@fortifex.com",
                    Subject = "Welcome to Fortifex",
                    Body = emailBodyStringBuilder.ToString()
                };

                await _emailService.SendEmailAsync(mailItem);

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Fortifex:TokenSecurityKey").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var claims = new[]
{
                            new Claim(ClaimTypes.Name, request.MemberUsername)
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
                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
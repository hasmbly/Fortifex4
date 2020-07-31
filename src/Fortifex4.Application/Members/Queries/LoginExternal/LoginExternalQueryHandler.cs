using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Members.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Members.Queries.LoginExternal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.Application.Members.Queries.LoginExternal
{
    public class LoginExternalQueryHandler : IRequestHandler<LoginExternalRequest, LoginExternalResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IConfiguration _config;

        public LoginExternalQueryHandler(IFortifex4DBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginExternalResponse> Handle(LoginExternalRequest request, CancellationToken cancellationToken)
        {
            var result = new LoginExternalResponse();

            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
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

                FortifexUtility.SplitFullName(request.FullName, out string firstName, out string lastName);

                member = new Member
                {
                    MemberUsername = request.MemberUsername,
                    FirstName = firstName,
                    LastName = lastName,
                    AuthenticationScheme = request.SchemeProvider,
                    ExternalID = request.ExternalID,
                    RegionID = defaultRegion.RegionID,
                    GenderID = GenderID.Default,
                    BirthDate = MemberBirthDate.Default,
                    PreferredFiatCurrencyID = defaultPreferredFiatCurrency.CurrencyID,
                    PreferredCoinCurrencyID = defaultPreferredCoinCurrency.CurrencyID,
                    PreferredTimeFrameID = TimeFrameID.Default,
                    PictureURL = request.PictureURL,
                    ActivationCode = Guid.NewGuid(),
                    ActivationStatus = ActivationStatus.Active
                };

                await _context.Members.AddAsync(member);
                await _context.SaveChangesAsync(cancellationToken);

                result.IsNewMember = true;
            }

            string securityKey = _config.GetSection("Fortifex:TokenSecurityKey").Value;

            result.Token = TokenHelper.GenerateToken(member, securityKey);

            return result;
        }
    }
}
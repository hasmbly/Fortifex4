using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.CreateExternalMember
{
    public class CreateExternalMemberCommand : IRequest<CreateExternalMemberResult>
    {
        public string ExternalID { get; set; }
        public string ClaimType { get; set; }
        public string FullName { get; set; }
        public string MemberUsername { get; set; }
        public string PictureURL { get; set; }
    }

    public class CreateExternalMemberCommandHandler : IRequestHandler<CreateExternalMemberCommand, CreateExternalMemberResult>
    {
        private readonly IFortifex4DBContext _context;

        public CreateExternalMemberCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CreateExternalMemberResult> Handle(CreateExternalMemberCommand request, CancellationToken cancellationToken)
        {
            var result = new CreateExternalMemberResult();

            if (await _context.Members.AnyAsync(x => x.MemberUsername == request.MemberUsername, cancellationToken))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Username already taken";
            }
            else if (await _context.Members.AnyAsync(x => x.ExternalID == request.ExternalID && x.AuthenticationScheme == request.ClaimType, cancellationToken))
            {
                result.IsSuccessful = false;
                result.ErrorMessage = $"ExternalID {request.ExternalID} in  {request.ClaimType} already exists";
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

                FortifexUtility.SplitFullName(request.FullName, out string firstName, out string lastName);

                var member = new Member
                {
                    MemberUsername = request.MemberUsername,
                    FirstName = firstName,
                    LastName = lastName,
                    AuthenticationScheme = request.ClaimType,
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

                result.IsSuccessful = true;
            }

            return result;
        }
    }
}
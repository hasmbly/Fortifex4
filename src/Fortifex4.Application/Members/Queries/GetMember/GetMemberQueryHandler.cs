using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Queries.GetMember
{
    public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, GetMemberResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetMemberQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetMemberResult> Handle(GetMemberQuery query, CancellationToken cancellationToken)
        {
            var member = await _context.Members.Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.Gender)
                .Include(a => a.Region).ThenInclude(b => b.Country)
                .Include(a => a.PreferredFiatCurrency)
                .Include(a => a.PreferredCoinCurrency)
                .Include(a => a.PreferredTimeFrame)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            var result = new GetMemberResult 
            {
                MemberUsername = member.MemberUsername,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate,
                ExternalID = member.ExternalID,
                PictureUrl = member.PictureURL,

                GenderID = member.GenderID,
                GenderName = member.Gender.Name,

                RegionID = member.Region.RegionID,
                RegionName = member.Region.Name,
                CountryCode = member.Region.CountryCode,
                CountryName = member.Region.Name,
                
                PreferredCoinCurrencyID = member.PreferredCoinCurrency.CurrencyID,
                PreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol,
                PreferredCoinCurrencyName = member.PreferredCoinCurrency.Name,

                PreferredFiatCurrencyID = member.PreferredFiatCurrency.CurrencyID,
                PreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                PreferredFiatCurrencyName = member.PreferredFiatCurrency.Name,
                
                PreferredTimeFrameID = member.PreferredTimeFrame.TimeFrameID,
                PreferredTimeFrameName = member.PreferredTimeFrame.Name,
            };

            return result;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.UpdatePreferredFiatCurrency
{
    public class UpdatePreferredFiatCurrencyCommandHandler : IRequestHandler<UpdatePreferredFiatCurrencyRequest, UpdatePreferredFiatCurrencyResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdatePreferredFiatCurrencyCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdatePreferredFiatCurrencyResponse> Handle(UpdatePreferredFiatCurrencyRequest request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == request.PreferredFiatCurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.PreferredFiatCurrencyID);

            if (currency.CurrencyType != CurrencyType.Fiat)
                throw new ArgumentException($"[{nameof(Currency)}] with key [{request.PreferredFiatCurrencyID}] is not a Fiat Currency");

            member.PreferredFiatCurrencyID = request.PreferredFiatCurrencyID;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePreferredFiatCurrencyResponse 
            { 
                IsSucessful = true
            };
        }
    }
}
﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Members.Commands.UpdatePreferredCoinCurrency
{
    public class UpdatePreferredCoinCurrencyCommand : IRequest
    {
        public string MemberUsername { get; set; }
        public int PreferredCoinCurrencyID { get; set; }
    }

    public class UpdatePreferredCoinCurrencyCommandHandler : IRequestHandler<UpdatePreferredCoinCurrencyCommand>
    {
        private readonly IFortifex4DBContext _context;

        public UpdatePreferredCoinCurrencyCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdatePreferredCoinCurrencyCommand request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == request.PreferredCoinCurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.PreferredCoinCurrencyID);

            if (currency.CurrencyType != CurrencyType.Coin)
                throw new ArgumentException($"[{nameof(Currency)}] with key [{request.PreferredCoinCurrencyID}] is not a Coin Currency");

            member.PreferredCoinCurrencyID = request.PreferredCoinCurrencyID;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesQuery : IRequest<GetAvailableCurrenciesResult>
    {
        public int OwnerID { get; set; }
    }

    public class GetAvailableCurrenciesQueryHandler : IRequestHandler<GetAvailableCurrenciesQuery, GetAvailableCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAvailableCurrenciesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAvailableCurrenciesResult> Handle(GetAvailableCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAvailableCurrenciesResult();

            var owner = await _context.Owners
                   .Where(x => x.OwnerID == request.OwnerID)
                   .Include(a => a.Wallets)
                    .ThenInclude(b => b.Pockets)
                   .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), request.OwnerID);

            List<int> usedCurrencyIDs = new List<int>();

            foreach (var wallet in owner.Wallets)
            {
                var mainPocket = wallet.Pockets.Single(x => x.IsMain);

                usedCurrencyIDs.Add(mainPocket.CurrencyID);
            }

            // Untuk saat ini, Exchange Wallet (Pocket) hanya boleh menggunakan Coin Currency
            // User tidak diperbolehkan lagi membuat Exchange Wallet yang berbasiskan Fiat Currency
            // User belum diperbolehkan membuat Exchange Wallet yang berbasiskan Token Currency
            var allCoinCurrencies = await _context.Currencies
                .Where(x => x.CurrencyType == CurrencyType.Coin)
                .OrderBy(x => x.Rank)
                .ToListAsync(cancellationToken);

            foreach (Currency currency in allCoinCurrencies)
            {
                if (!usedCurrencyIDs.Any(x => x == currency.CurrencyID))
                {
                    CurrencyDTO currencyDTO = new CurrencyDTO
                    {
                        CurrencyID = currency.CurrencyID,
                        Symbol = currency.Symbol,
                        Name = currency.Name
                    };

                    result.Currencies.Add(currencyDTO);
                }
            }

            return result;
        }
    }
}
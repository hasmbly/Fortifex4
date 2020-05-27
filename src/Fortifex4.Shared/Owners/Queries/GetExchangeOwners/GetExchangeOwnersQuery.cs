﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Owners.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Owners.Queries.GetExchangeOwners
{
    public class GetExchangeOwnersQuery : IRequest<GetExchangeOwnersResult>
    {
        public string MemberUsername { get; set; }
    }

    public class GetExchangeOwnersQueryHandler : IRequestHandler<GetExchangeOwnersQuery, GetExchangeOwnersResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetExchangeOwnersQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetExchangeOwnersResult> Handle(GetExchangeOwnersQuery request, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == request.MemberUsername)
                .Include(a => a.PreferredFiatCurrency)
                .Include(a => a.PreferredCoinCurrency)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), request.MemberUsername);

            var result = new GetExchangeOwnersResult
            {
                MemberPreferredFiatCurrencyID = member.PreferredFiatCurrencyID,
                MemberPreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                MemberPreferredFiatCurrencyUnitPriceInUSD = member.PreferredFiatCurrency.UnitPriceInUSD,
                MemberPreferredCoinCurrencyID = member.PreferredCoinCurrencyID,
                MemberPreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol,
                MemberPreferredCoinCurrencyUnitPriceInUSD = member.PreferredCoinCurrency.UnitPriceInUSD,
            };

            var exchangeOwners = await _context.Owners
                .Where(x =>
                    x.MemberUsername == request.MemberUsername &&
                    x.ProviderType == ProviderType.Exchange)
                .Include(a => a.Provider)
                .Include(a => a.Wallets)
                    .ThenInclude(b => b.Blockchain)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var exchangeOwner in exchangeOwners)
            {
                ExchangeOwnerDTO exchangeOwnerDTO = new ExchangeOwnerDTO
                {
                    OwnerID = exchangeOwner.OwnerID,
                    ProviderID = exchangeOwner.ProviderID,
                    ProviderName = exchangeOwner.Provider.Name,
                    ProviderSiteURL = exchangeOwner.Provider.SiteURL,
                    ExchangeWallets = new List<WalletDTO>()
                };

                foreach (var exchangeWallet in exchangeOwner.Wallets)
                {
                    var mainPocket = await _context.Pockets
                        .Where(x => x.WalletID == exchangeWallet.WalletID && x.IsMain)
                            .Include(c => c.Currency)
                            .Include(c => c.Transactions)
                        .AsNoTracking()
                        .SingleAsync(cancellationToken);

                    WalletDTO walletDTO = new WalletDTO
                    {
                        WalletID = exchangeWallet.WalletID,
                        Name = exchangeWallet.Name,
                        Address = exchangeWallet.Address,
                        BlockchainName = exchangeWallet.Blockchain.Name,
                        MainPocketCurrencySymbol = mainPocket.Currency.Symbol,
                        MainPocketCurrencyName = mainPocket.Currency.Name,
                        MainPocketCurrencyType = mainPocket.CurrencyType,
                        MainPocketCurrencyUnitPriceInUSD = mainPocket.Currency.UnitPriceInUSD,
                        MainPocketBalance = mainPocket.Transactions.Sum(x => x.Amount),
                        Container = result
                    };

                    exchangeOwnerDTO.ExchangeWallets.Add(walletDTO);
                }

                result.ExchangeOwners.Add(exchangeOwnerDTO);
            }

            return result;
        }
    }
}
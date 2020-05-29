using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Owners.Queries.GetOwner;
using Fortifex4.Shared.Owners.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Owners.Queries.GetOwner
{
    public class GetOwnerQueryHandler : IRequestHandler<GetOwnerRequest, GetOwnerResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetOwnerQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetOwnerResponse> Handle(GetOwnerRequest request, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                    .Include(a => a.Provider)
                    .Include(a => a.Member).ThenInclude(b => b.PreferredFiatCurrency)
                    .Include(a => a.Member).ThenInclude(b => b.PreferredCoinCurrency)
                    .Include(a => a.Wallets).ThenInclude(b => b.Blockchain)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), request.OwnerID);

            var result = new GetOwnerResponse
            {
                OwnerID = owner.OwnerID,
                MemberUsername = owner.MemberUsername,
                ProviderID = owner.ProviderID,
                MemberPreferredFiatCurrencyID = owner.Member.PreferredFiatCurrencyID,
                MemberPreferredFiatCurrencySymbol = owner.Member.PreferredFiatCurrency.Symbol,
                MemberPreferredFiatCurrencyUnitPriceInUSD = owner.Member.PreferredFiatCurrency.UnitPriceInUSD,
                MemberPreferredCoinCurrencyID = owner.Member.PreferredCoinCurrencyID,
                MemberPreferredCoinCurrencySymbol = owner.Member.PreferredCoinCurrency.Symbol,
                MemberPreferredCoinCurrencyUnitPriceInUSD = owner.Member.PreferredCoinCurrency.UnitPriceInUSD,
                ProviderName = owner.Provider.Name,
                ProviderSiteURL = owner.Provider.SiteURL
            };

            var cryptoWallets = await _context.Wallets
                .Where(x =>
                    x.OwnerID == request.OwnerID &&
                    x.BlockchainID != BlockchainID.Fiat)
                .Include(b => b.Blockchain)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var wallet in cryptoWallets)
            {
                var mainPocket = await _context.Pockets
                    .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                        .Include(c => c.Currency)
                        .Include(c => c.Transactions)
                    .AsNoTracking()
                    .SingleAsync(cancellationToken);

                WalletDTO walletDTO = new WalletDTO
                {
                    WalletID = wallet.WalletID,
                    Name = wallet.Name,
                    Address = wallet.Address,
                    BlockchainName = wallet.Blockchain.Name,
                    MainPocketCurrencySymbol = mainPocket.Currency.Symbol,
                    MainPocketCurrencyName = mainPocket.Currency.Name,
                    MainPocketCurrencyType = mainPocket.CurrencyType,
                    MainPocketCurrencyUnitPriceInUSD = mainPocket.Currency.UnitPriceInUSD,
                    MainPocketBalance = mainPocket.Transactions.Sum(x => x.Amount),
                    Container = result
                };

                result.Wallets.Add(walletDTO);
            }

            return result;
        }
    }
}
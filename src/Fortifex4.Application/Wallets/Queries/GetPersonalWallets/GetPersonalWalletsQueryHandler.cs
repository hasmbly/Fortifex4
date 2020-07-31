using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Queries.GetPersonalWallets
{
    public class GetPersonalWalletsQueryHandler : IRequestHandler<GetPersonalWalletsRequest, GetPersonalWalletsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetPersonalWalletsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetPersonalWalletsResponse> Handle(GetPersonalWalletsRequest query, CancellationToken cancellationToken)
        {
            var member = await _context.Members
                .Where(x => x.MemberUsername == query.MemberUsername)
                    .Include(a => a.PreferredFiatCurrency)
                    .Include(a => a.PreferredCoinCurrency)
                .SingleOrDefaultAsync(cancellationToken);

            if (member == null)
                throw new NotFoundException(nameof(Member), query.MemberUsername);

            var result = new GetPersonalWalletsResponse
            {
                MemberPreferredFiatCurrencySymbol = member.PreferredFiatCurrency.Symbol,
                MemberPreferredFiatCurrencyUnitPriceInUSD = member.PreferredFiatCurrency.UnitPriceInUSD,
                MemberPreferredCoinCurrencySymbol = member.PreferredCoinCurrency.Symbol,
                MemberPreferredCoinCurrencyUnitPriceInUSD = member.PreferredCoinCurrency.UnitPriceInUSD,
            };

            var owners = await _context.Owners
                .Where(x =>
                    x.MemberUsername == query.MemberUsername &&
                    x.ProviderType == ProviderType.Personal)
                .ToListAsync(cancellationToken);

            foreach (Owner owner in owners)
            {
                var wallets = await _context.Wallets.Where(x => x.OwnerID == owner.OwnerID)
                    .Include(a => a.Blockchain)
                    .ToListAsync(cancellationToken);

                foreach (Wallet wallet in wallets)
                {
                    var mainPocket = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                            .Include(a => a.Currency)
                            .Include(a => a.Transactions)
                        .SingleAsync(cancellationToken);

                    WalletDTO walletDTO = new WalletDTO
                    {
                        WalletID = wallet.WalletID,
                        BlockchainID = wallet.BlockchainID,
                        OwnerID = wallet.OwnerID,
                        ProviderType = ProviderType.Personal,
                        Name = wallet.Name,
                        Address = wallet.Address,
                        IsSynchronized = wallet.IsSynchronized,
                        BlockchainSymbol = wallet.Blockchain.Symbol,
                        BlockchainName = wallet.Blockchain.Name,
                        MainPocketCurrencyUnitPriceInUSD = mainPocket.Currency.UnitPriceInUSD,
                        MainPocketBalance = mainPocket.Transactions.Sum(x => x.Amount),
                        //Container = result
                    };

                    #region MainPocketBalanceInPreferredFiatCurrency
                    if (result.MemberPreferredFiatCurrencyUnitPriceInUSD > 0)
                        walletDTO.MainPocketBalanceInPreferredFiatCurrency = walletDTO.MainPocketBalance * (walletDTO.MainPocketCurrencyUnitPriceInUSD / result.MemberPreferredFiatCurrencyUnitPriceInUSD);
                    else
                        walletDTO.MainPocketBalanceInPreferredFiatCurrency = 0m;
                    #endregion

                    #region MainPocketBalanceInPreferredCoinCurrency
                    if (result.MemberPreferredCoinCurrencyUnitPriceInUSD > 0)
                        walletDTO.MainPocketBalanceInPreferredCoinCurrency = walletDTO.MainPocketBalance * (walletDTO.MainPocketCurrencyUnitPriceInUSD / result.MemberPreferredCoinCurrencyUnitPriceInUSD);
                    else
                        walletDTO.MainPocketBalanceInPreferredCoinCurrency = 0m;
                    #endregion

                    result.PersonalWallets.Add(walletDTO);
                }
            }


            result.IsSuccessful = true;

            return result;
        }
    }
}
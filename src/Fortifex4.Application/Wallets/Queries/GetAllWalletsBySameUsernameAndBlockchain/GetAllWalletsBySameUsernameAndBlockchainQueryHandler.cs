using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.Shared.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Queries.GetAllWalletsBySameUsernameAndBlockchain
{
    public class GetAllWalletsBySameUsernameAndBlockchainQueryHandler : IRequestHandler<GetAllWalletsBySameUsernameAndBlockchainRequest, GetAllWalletsBySameUsernameAndBlockchainResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllWalletsBySameUsernameAndBlockchainQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllWalletsBySameUsernameAndBlockchainResponse> Handle(GetAllWalletsBySameUsernameAndBlockchainRequest query, CancellationToken cancellationToken)
        {
            var result = new GetAllWalletsBySameUsernameAndBlockchainResponse();

            var owners = await _context.Owners
                .Where(x => x.MemberUsername == query.MemberUsername)
                .Include(a => a.Provider)
                .ToListAsync(cancellationToken);

            if (owners == null)
                throw new NotFoundException(nameof(Owner), query.MemberUsername);

            foreach (Owner owner in owners)
            {
                var wallets = await _context.Wallets
                    .Where(x =>
                        x.OwnerID == owner.OwnerID &&
                        !x.IsSynchronized)
                    .ToListAsync(cancellationToken);

                foreach (Wallet wallet in wallets)
                {
                    var pocket = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.IsMain && x.CurrencyType == CurrencyType.Coin)
                        .Include(a => a.Currency)
                        .AsNoTracking()
                        .SingleOrDefaultAsync(cancellationToken);

                    if (pocket != null)
                    {
                        result.Wallets.Add(new WalletSameCurrencyDTO
                        {
                            OwnerProviderName = wallet.Owner.Provider.Name,
                            WalletID = wallet.WalletID,
                            Name = wallet.Name,
                            PocketID = pocket.PocketID,
                            CurrencySymbol = pocket.Currency.Symbol,
                            CurrencyName = pocket.Currency.Name,
                            ProviderType = wallet.ProviderType
                        });
                    }
                }
            }

            if (result.Wallets.Count > 0)
                result.IsSuccessful = true;

            return result;
        }
    }
}
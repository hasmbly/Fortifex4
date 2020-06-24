using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Wallets.Common;
using Fortifex4.Shared.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Queries.GetWalletsBySameUsernameAndBlockchain
{
    public class GetWalletsBySameUsernameAndBlockchainQueryHandler : IRequestHandler<GetWalletsBySameUsernameAndBlockchainRequest, GetWalletsBySameUsernameAndBlockchainResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetWalletsBySameUsernameAndBlockchainQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetWalletsBySameUsernameAndBlockchainResponse> Handle(GetWalletsBySameUsernameAndBlockchainRequest query, CancellationToken cancellationToken)
        {
            var result = new GetWalletsBySameUsernameAndBlockchainResponse();

            var sourceWallet = await _context.Wallets
                .Where(x => x.WalletID == query.WalletID)
                .Include(a => a.Owner)
                .SingleOrDefaultAsync(cancellationToken);

            if (sourceWallet == null)
                throw new NotFoundException(nameof(Wallet), query.WalletID);

            var owners = await _context.Owners
                .Where(x => x.MemberUsername == sourceWallet.Owner.MemberUsername)
                .Include(a => a.Provider)
                .ToListAsync(cancellationToken);

            foreach (Owner owner in owners)
            {
                var wallets = await _context.Wallets
                    .Where(x =>
                        x.WalletID != sourceWallet.WalletID &&
                        x.BlockchainID == sourceWallet.BlockchainID &&
                        x.OwnerID == owner.OwnerID &&
                        !x.IsSynchronized)
                    .ToListAsync(cancellationToken);

                foreach (Wallet wallet in wallets)
                {
                    var pocket = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                        .SingleAsync(cancellationToken);

                    result.Wallets.Add(new WalletSameCurrencyDTO
                    {
                        OwnerProviderName = wallet.Owner.Provider.Name,
                        WalletID = wallet.WalletID,
                        Name = wallet.Name,
                        PocketID = pocket.PocketID,
                        ProviderType = wallet.ProviderType
                    });
                }
            }

            if (result.Wallets.Count > 0)
                result.IsSuccessful = true;

            return result;
        }
    }
}
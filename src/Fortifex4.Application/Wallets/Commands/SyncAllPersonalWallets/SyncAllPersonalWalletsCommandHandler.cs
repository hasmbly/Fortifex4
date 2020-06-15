using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Application.Common.Interfaces.Dogecoin;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Application.Common.Interfaces.Hive;
using Fortifex4.Application.Common.Interfaces.Steem;
using Fortifex4.Application.Wallets.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Wallets.Commands.SyncAllPersonalWallets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.SyncAllPersonalWallets
{
    public class SyncAllPersonalWalletsCommandHandler : IRequestHandler<SyncAllPersonalWalletsRequest, SyncAllPersonalWalletsResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;
        private readonly IBitcoinService _bitcoinService;
        private readonly IEthereumService _ethereumService;
        private readonly IDogecoinService _dogecoinService;
        private readonly ISteemService _steemService;
        private readonly IHiveService _hiveService;

        public SyncAllPersonalWalletsCommandHandler
        (
            IFortifex4DBContext context,
            IDateTimeOffsetService dateTimeOffset,
            IBitcoinService bitcoinService,
            IEthereumService ethereumService,
            IDogecoinService dogecoinService,
            ISteemService steemService,
            IHiveService hiveService
        )
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
            _bitcoinService = bitcoinService;
            _ethereumService = ethereumService;
            _dogecoinService = dogecoinService;
            _steemService = steemService;
            _hiveService = hiveService;
        }

        public async Task<SyncAllPersonalWalletsResponse> Handle(SyncAllPersonalWalletsRequest request, CancellationToken cancellationToken)
        {
            var result = new SyncAllPersonalWalletsResponse();

            var synchronizedWallets = await _context.Wallets
                .Where(x => x.IsSynchronized)
                .Include(x => x.Blockchain)
                .ToListAsync(cancellationToken);

            foreach (var wallet in synchronizedWallets)
            {
                var cryptoWallet = new CryptoWallet();

                if (wallet.Blockchain.Symbol == CurrencySymbol.BTC)
                {
                    cryptoWallet = await _bitcoinService.GetBitcoinWalletAsync(wallet.Address);
                }
                else if (wallet.Blockchain.Symbol == CurrencySymbol.ETH)
                {
                    cryptoWallet = await _ethereumService.GetEthereumWalletAsync(wallet.Address);
                }
                else if (wallet.Blockchain.Symbol == CurrencySymbol.DOGE)
                {
                    cryptoWallet = await _dogecoinService.GetDogecoinWalletAsync(wallet.Address);
                }
                else if (wallet.Blockchain.Symbol == CurrencySymbol.STEEM)
                {
                    cryptoWallet = await _steemService.GetSteemWalletAsync(wallet.Address);
                }
                else if (wallet.Blockchain.Symbol == CurrencySymbol.HIVE)
                {
                    cryptoWallet = await _hiveService.GetHiveWalletAsync(wallet.Address);
                }

                var walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);

                result.Wallets.Add(walletDTO);
            }

            return result;
        }
    }
}
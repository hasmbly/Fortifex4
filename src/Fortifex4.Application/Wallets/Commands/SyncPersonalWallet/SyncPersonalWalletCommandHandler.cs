using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Application.Common.Interfaces.Dogecoin;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Application.Common.Interfaces.Hive;
using Fortifex4.Application.Common.Interfaces.Steem;
using Fortifex4.Application.Wallets.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using Fortifex4.Shared.Wallets.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.SyncPersonalWallet
{
    public class SyncPersonalWalletCommandHandler : IRequestHandler<SyncPersonalWalletRequest, SyncPersonalWalletResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;
        private readonly IBitcoinService _bitcoinService;
        private readonly IEthereumService _ethereumService;
        private readonly IDogecoinService _dogecoinService;
        private readonly ISteemService _steemService;
        private readonly IHiveService _hiveService;

        public SyncPersonalWalletCommandHandler(
            IFortifex4DBContext context,
            IDateTimeOffsetService dateTimeOffset,
            IBitcoinService bitcoinService,
            IEthereumService ethereumService,
            IDogecoinService dogecoinService,
            ISteemService steemService,
            IHiveService hiveService)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
            _bitcoinService = bitcoinService;
            _ethereumService = ethereumService;
            _dogecoinService = dogecoinService;
            _steemService = steemService;
            _hiveService = hiveService;
        }

        public async Task<SyncPersonalWalletResponse> Handle(SyncPersonalWalletRequest request, CancellationToken cancellationToken)
        {
            var result = new SyncPersonalWalletResponse();

            var wallet = await _context.Wallets
                .Where(x => x.WalletID == request.WalletID)
                .Include(x => x.Blockchain)
                .SingleOrDefaultAsync(cancellationToken);

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            var cryptoWallet = new CryptoWallet();
            var walletDTO = new WalletDTO();

            if (wallet.Blockchain.Symbol == CurrencySymbol.BTC)
            {
                cryptoWallet = await _bitcoinService.GetBitcoinWalletAsync(wallet.Address);
                walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.ETH)
            {
                if (wallet.IsSynchronized)
                {
                    walletDTO = await WalletSynchronizer.ImportEthereumTransactions(_context, _ethereumService, wallet, cancellationToken);
                }
                else
                {
                    cryptoWallet = await _ethereumService.GetEthereumWalletAsync(wallet.Address);
                    walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);
                }
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.DOGE)
            {
                cryptoWallet = await _dogecoinService.GetDogecoinWalletAsync(wallet.Address);
                walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.STEEM)
            {
                cryptoWallet = await _steemService.GetSteemWalletAsync(wallet.Address);
                walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.HIVE)
            {
                cryptoWallet = await _hiveService.GetHiveWalletAsync(wallet.Address);
                walletDTO = await WalletSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, cryptoWallet, cancellationToken);
            }

            result.IsSuccessful = true;
            result.Wallet = walletDTO;

            return result;
        }
    }
}
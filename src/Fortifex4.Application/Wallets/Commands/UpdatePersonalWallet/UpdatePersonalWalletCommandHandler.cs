using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Wallets.Commands.UpdatePersonalWallet
{
    public class UpdatePersonalWalletCommandHandler : IRequestHandler<UpdatePersonalWalletRequest, UpdatePersonalWalletResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdatePersonalWalletCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdatePersonalWalletResponse> Handle(UpdatePersonalWalletRequest request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets.FindAsync(request.WalletID);

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            wallet.Name = request.Name;

            // Property lain selain Name hanya bisa diubah kalau Wallet tidak synchronized
            if (!wallet.IsSynchronized)
            {
                wallet.Address = request.Address;

                var mainPocket = await _context.Pockets
                    .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                    .Include(a => a.Transactions)
                    .SingleOrDefaultAsync(cancellationToken);

                mainPocket.Address = request.Address;

                // Kalau wallet ini mau ganti Blockchain
                if (wallet.BlockchainID != request.BlockchainID)
                {
                    bool hasInternalTransfers = mainPocket.Transactions.Any(x =>
                        x.TransactionType == TransactionType.InternalTransferIN ||
                        x.TransactionType == TransactionType.InternalTransferOUT);

                    // Boleh ganti Blockchain hanya jika belum pernah ada transaksi Internal Transfer
                    if (!hasInternalTransfers)
                    {
                        wallet.BlockchainID = request.BlockchainID;

                        var currency = await _context.Currencies
                            .Where(x =>
                                x.BlockchainID == request.BlockchainID &&
                                x.CurrencyType == CurrencyType.Coin)
                                .SingleOrDefaultAsync(cancellationToken);

                        mainPocket.CurrencyID = currency.CurrencyID;
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePersonalWalletResponse 
            {
                IsSucessful = true
            };
        }
    }
}
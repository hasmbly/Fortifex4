using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.DeleteWallet
{
    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletRequest, DeleteWalletResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteWalletCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteWalletResponse> Handle(DeleteWalletRequest request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .Where(x => x.WalletID == request.WalletID)
                .Include(a => a.Pockets)
                    .ThenInclude(b => b.Transactions)
                    .ThenInclude(c => c.FromInternalTransfers)
                    .ThenInclude(d => d.ToTransaction)
                .Include(a => a.Pockets)
                    .ThenInclude(b => b.Transactions)
                    .ThenInclude(c => c.ToInternalTransfers)
                    .ThenInclude(d => d.FromTransaction)
                .Include(a => a.Pockets)
                    .ThenInclude(b => b.Transactions)
                    .ThenInclude(c => c.FromTrades)
                    .ThenInclude(d => d.ToTransaction)
                .Include(a => a.Pockets)
                    .ThenInclude(b => b.Transactions)
                    .ThenInclude(c => c.ToTrades)
                    .ThenInclude(d => d.FromTransaction)
                .SingleOrDefaultAsync();

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            //Deleting Transactions
            foreach (var pocket in wallet.Pockets)
            {
                foreach (var transaction in pocket.Transactions)
                {
                    _context.InternalTransfers.RemoveRange(transaction.FromInternalTransfers);
                    _context.Transactions.RemoveRange(transaction.FromInternalTransfers.Select(x => x.ToTransaction));
                    _context.InternalTransfers.RemoveRange(transaction.ToInternalTransfers);
                    _context.Transactions.RemoveRange(transaction.ToInternalTransfers.Select(x => x.FromTransaction));
                    _context.Trades.RemoveRange(transaction.FromTrades);
                    _context.Trades.RemoveRange(transaction.ToTrades);
                }

                _context.Transactions.RemoveRange(pocket.Transactions);
            }

            _context.Pockets.RemoveRange(wallet.Pockets);
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWalletResponse 
            { 
                IsSucessful = true
            };
        }
    }
}
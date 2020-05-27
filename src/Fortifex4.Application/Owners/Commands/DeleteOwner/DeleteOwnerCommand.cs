using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerCommand : IRequest
    {
        public int OwnerID { get; set; }
    }

    public class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteOwnerCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                .Include(a => a.Wallets)
                    .ThenInclude(b => b.Pockets)
                    .ThenInclude(c => c.Transactions)
                    .ThenInclude(d => d.FromInternalTransfers)
                    .ThenInclude(d => d.ToTransaction)
                .Include(a => a.Wallets)
                    .ThenInclude(b => b.Pockets)
                    .ThenInclude(c => c.Transactions)
                    .ThenInclude(d => d.ToInternalTransfers)
                    .ThenInclude(d => d.FromTransaction)
                .Include(a => a.Wallets)
                    .ThenInclude(b => b.Pockets)
                    .ThenInclude(c => c.Transactions)
                    .ThenInclude(d => d.FromTrades)
                .Include(a => a.Wallets)
                    .ThenInclude(b => b.Pockets)
                    .ThenInclude(c => c.Transactions)
                    .ThenInclude(d => d.ToTrades)
                .SingleOrDefaultAsync();

            if (owner == null)
                throw new NotFoundException(nameof(Owner), request.OwnerID);

            foreach (var wallet in owner.Wallets)
            {
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
            }

            _context.Wallets.RemoveRange(owner.Wallets);
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
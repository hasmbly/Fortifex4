using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.System.Commands.RemoveAllTransactions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.System.Commands.RemoveAllTransactions
{
    public class RemoveAllTransactionsCommandHandler : IRequestHandler<RemoveAllTransactionsRequest, RemoveAllTransactionsResponse>
    {
        private readonly IFortifex4DBContext _context;

        public RemoveAllTransactionsCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<RemoveAllTransactionsResponse> Handle(RemoveAllTransactionsRequest request, CancellationToken cancellationToken)
        {
            int transactionsRemoved = await _context.Transactions.CountAsync(cancellationToken);

            //var wallets = await _context.Wallets
            //    .Include(a => a.Pockets)
            //        .ThenInclude(b => b.Transactions)
            //        .ThenInclude(c => c.FromInternalTransfers)
            //    .Include(a => a.Pockets)
            //        .ThenInclude(b => b.Transactions)
            //        .ThenInclude(c => c.FromTrades)
            //    .ToListAsync(cancellationToken);

            //foreach (var wallet in wallets)
            //{
            //    foreach (var pocket in wallet.Pockets)
            //    {
            //        foreach (var transaction in pocket.Transactions)
            //        {
            //            _context.InternalTransfers.RemoveRange(transaction.FromInternalTransfers);
            //            _context.Trades.RemoveRange(transaction.FromTrades);
            //            _context.Trades.RemoveRange(transaction.ToTrades);
            //        }

            //        transactionsRemoved += pocket.Transactions.Count;
            //        _context.Transactions.RemoveRange(pocket.Transactions);
            //    }
            //}

            _context.InternalTransfers.RemoveRange(_context.InternalTransfers);
            _context.Trades.RemoveRange(_context.Trades);
            _context.Transactions.RemoveRange(_context.Transactions);
            await _context.SaveChangesAsync(cancellationToken);

            return new RemoveAllTransactionsResponse 
            {
                IsSuccessful = true,
                TransactionsRemoved = transactionsRemoved
            };
        }
    }
}
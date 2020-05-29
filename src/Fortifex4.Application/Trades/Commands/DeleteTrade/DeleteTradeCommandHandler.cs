using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Trades.Commands.DeleteTrade;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Trades.Commands.DeleteTrade
{
    public class DeleteTradeCommandHandler : IRequestHandler<DeleteTradeRequest, DeleteTradeResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteTradeCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteTradeResponse> Handle(DeleteTradeRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteTradeResponse
            {
                IsSuccessful = false
            };

            var trade = await _context.Trades
                .Where(x => x.TradeID == request.TradeID)
                .SingleOrDefaultAsync();

            if (trade == null)
                throw new NotFoundException(nameof(Trade), request.TradeID);

            // Delete Trade
            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync(cancellationToken);

            // Delete source Transactions
            var sourceTransaction = await _context.Transactions
                .Where(x => x.TransactionID == trade.FromTransactionID)
                .SingleOrDefaultAsync();

            _context.Transactions.Remove(sourceTransaction);
            await _context.SaveChangesAsync(cancellationToken);

            // Delete destination Transactions
            var destinationTransaction = await _context.Transactions
                .Where(x => x.TransactionID == trade.ToTransactionID)
                .SingleOrDefaultAsync();

            _context.Transactions.Remove(destinationTransaction);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
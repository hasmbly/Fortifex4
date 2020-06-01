using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Trades.Queries.GetTrade;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Trades.Queries.GetTrade
{
    public class GetTradeQueryHandler : IRequestHandler<GetTradeRequest, GetTradeResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetTradeQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetTradeResponse> Handle(GetTradeRequest query, CancellationToken cancellationToken)
        {
            Trade trade = await _context.Trades
                .Where(x => x.TradeID == query.TradeID)
                .Include(a => a.FromTransaction)
                    .ThenInclude(b => b.Pocket)
                    .ThenInclude(c => c.Wallet)
                .Include(a => a.ToTransaction)
                    .ThenInclude(b => b.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (trade == null)
                throw new NotFoundException(nameof(Trade), query.TradeID);

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == trade.FromTransactionID)
                .SingleOrDefaultAsync(cancellationToken);

            if (trade == null)
                throw new NotFoundException(nameof(Transaction), trade.FromTransactionID);

            var result = new GetTradeResponse
            {
                IsSuccessful = true,
                TradeID = trade.TradeID,
                OwnerID = trade.FromTransaction.Pocket.Wallet.OwnerID,
                SourceCurrencyID = trade.FromTransaction.Pocket.CurrencyID,
                DestinationCurrencyID = trade.ToTransaction.Pocket.CurrencyID,
                Amount = trade.FromTransaction.Amount,
                TransactionDateTime = trade.FromTransaction.TransactionDateTime,
                TradeType = trade.TradeType,
                UnitPrice = trade.UnitPrice,
                UnitPriceInUSD = transaction.UnitPriceInUSD,
                IsWithholding = trade.IsWithholding
            };

            return result;
        }
    }
}
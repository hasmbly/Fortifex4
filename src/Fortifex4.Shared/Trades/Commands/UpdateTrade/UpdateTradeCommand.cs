using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Trades.Commands.UpdateTrade
{
    public class UpdateTradeCommand : IRequest<UpdateTradeResult>
    {
        public int TradeID { get; set; }
        public TradeType TradeType { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInUSD { get; set; }
    }

    public class UpdateTradeCommandHandler : IRequestHandler<UpdateTradeCommand, UpdateTradeResult>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateTradeCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateTradeResult> Handle(UpdateTradeCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateTradeResult
            {
                IsSuccessful = false
            };

            var trade = await _context.Trades
                .Where(x => x.TradeID == request.TradeID)
                .Include(a => a.FromTransaction)
                .Include(a => a.ToTransaction)
                .SingleOrDefaultAsync(cancellationToken);

            if (trade == null)
                throw new NotFoundException(nameof(Trade), request.TradeID);

            decimal unitPrice = decimal.Round(request.UnitPrice, 4);
            decimal totalPrice = request.Amount * unitPrice;

            trade.UnitPrice = unitPrice;
            trade.FromTransaction.TransactionDateTime = request.TransactionDateTime;
            trade.FromTransaction.UnitPriceInUSD = request.UnitPriceInUSD;
            trade.ToTransaction.TransactionDateTime = request.TransactionDateTime;
            trade.ToTransaction.UnitPriceInUSD = request.UnitPriceInUSD;

            if (request.TradeType == TradeType.Buy)
            {
                trade.FromTransaction.Amount = request.Amount;
                trade.ToTransaction.Amount = trade.IsWithholding ? -totalPrice : 0m;
            }
            else if (request.TradeType == TradeType.Sell)
            {
                trade.FromTransaction.Amount = -request.Amount;
                trade.ToTransaction.Amount = trade.IsWithholding ? totalPrice : 0m;
            }

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
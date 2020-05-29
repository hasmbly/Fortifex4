using System;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.Trades.Commands.UpdateTrade
{
    public class UpdateTradeRequest : IRequest<UpdateTradeResponse>
    {
        public int TradeID { get; set; }
        public TradeType TradeType { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInUSD { get; set; }
    }
}
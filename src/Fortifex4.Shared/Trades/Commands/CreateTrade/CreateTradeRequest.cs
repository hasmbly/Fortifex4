using System;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.Trades.Commands.CreateTrade
{
    public class CreateTradeRequest: IRequest<CreateTradeResponse>
    {
        public int OwnerID { get; set; }
        public int FromCurrencyID { get; set; }
        public int ToCurrencyID { get; set; }
        public decimal Amount { get; set; }
        public TradeType TradeType { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public bool IsWithholding { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Shared.Trades.Commands.DeleteTrade
{
    public class DeleteTradeRequest : IRequest<DeleteTradeResponse>
    {
        public int TradeID { get; set; }
    }
}
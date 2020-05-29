using MediatR;

namespace Fortifex4.Shared.Trades.Queries.GetTrade
{
    public class GetTradeRequest : IRequest<GetTradeResponse>
    {
        public int TradeID { get; set; }
    }
}
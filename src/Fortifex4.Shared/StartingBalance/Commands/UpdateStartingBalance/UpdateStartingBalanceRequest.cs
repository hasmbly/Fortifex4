using MediatR;

namespace Fortifex4.Shared.StartingBalance.Commands.UpdateStartingBalance
{
    public class UpdateStartingBalanceRequest : IRequest<UpdateStartingBalanceResponse>
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
    }
}
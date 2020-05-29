using MediatR;

namespace Fortifex4.Shared.Sync.Commands.UpdateSync
{
    public class UpdateSyncRequest : IRequest<UpdateSyncResponse>
    {
        public int TransactionID { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string PairWalletName { get; set; }
    }
}
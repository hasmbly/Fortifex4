using MediatR;

namespace Fortifex4.Application.Sync.Commands.UpdateSync
{
    public class UpdateSyncCommand : IRequest<UpdateSyncResult>
    {
        public int TransactionID { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string PairWalletName { get; set; }
    }
}
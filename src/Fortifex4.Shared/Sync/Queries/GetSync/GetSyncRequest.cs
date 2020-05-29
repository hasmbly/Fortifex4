using MediatR;

namespace Fortifex4.Shared.Sync.Queries.GetSync
{
    public class GetSyncRequest : IRequest<GetSyncResponse>
    {
        public int TransactionID { get; set; }
    }
}
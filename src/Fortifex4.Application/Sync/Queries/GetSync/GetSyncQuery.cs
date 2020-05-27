using MediatR;

namespace Fortifex4.Application.Sync.Queries.GetSync
{
    public class GetSyncQuery : IRequest<GetSyncResult>
    {
        public int TransactionID { get; set; }
    }
}
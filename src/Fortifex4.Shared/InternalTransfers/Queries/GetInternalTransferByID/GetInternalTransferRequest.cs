using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer
{
    public class GetInternalTransferRequest : IRequest<GetInternalTransferResponse>
    {
        public int InternalTransferID { get; set; }
    }
}
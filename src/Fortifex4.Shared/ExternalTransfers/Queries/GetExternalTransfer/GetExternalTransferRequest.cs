using MediatR;

namespace Fortifex4.Shared.ExternalTransfers.Queries.GetExternalTransfer
{
    public class GetExternalTransferRequest : IRequest<GetExternalTransferResponse>
    {
        public int TransactionID { get; set; }
    }
}
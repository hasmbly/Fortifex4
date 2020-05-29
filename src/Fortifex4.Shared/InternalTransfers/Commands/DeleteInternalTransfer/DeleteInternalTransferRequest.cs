using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer
{
    public class DeleteInternalTransferRequest : IRequest<DeleteInternalTransferResponse>
    {
        public int InternalTransfersID { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Application.InternalTransfers.Commands.DeleteInternalTransfer
{
    public class DeleteInternalTransferCommand : IRequest<DeleteInternalTransferResult>
    {
        public int InternalTransfersID { get; set; }
    }

}
using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.DeleteExternalTransfer
{
    public class DeleteExternalTransferRequest : IRequest<DeleteExternalTransferResponse>
    {
        public int TransactionID { get; set; }
    }
}
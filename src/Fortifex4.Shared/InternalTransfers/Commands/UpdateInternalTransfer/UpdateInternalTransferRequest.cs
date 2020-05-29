using System;
using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer
{
    public class UpdateInternalTransferRequest : IRequest<UpdateInternalTransferResponse>
    {
        public int InternalTransferID { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
    }
}
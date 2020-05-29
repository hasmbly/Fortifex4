using System;
using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer
{
    public class CreateInternalTransferRequest : IRequest<CreateInternalTransferResponse>
    {
        public int FromPocketID { get; set; }
        public int ToPocketID { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
    }
}
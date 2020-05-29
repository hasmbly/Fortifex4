using System;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.ExternalTransfers.Commands.UpdateExternalTransfer
{
    public class UpdateExternalTransferRequest : IRequest<UpdateExternalTransferResponse>
    {
        public int TransactionID { get; set; }
        public TransferDirection TransferDirection { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }
    }
}
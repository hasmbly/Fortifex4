using System;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer
{
    public class CreateExternalTransferRequest : IRequest<CreateExternalTransferResponse>
    {
        public int WalletID { get; set; }
        public TransferDirection TransferDirection { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}
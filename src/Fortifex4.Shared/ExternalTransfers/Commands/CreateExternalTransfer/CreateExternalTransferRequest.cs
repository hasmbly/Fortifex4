using System;
using System.ComponentModel.DataAnnotations;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;
using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer
{
    public class CreateExternalTransferRequest : IRequest<CreateExternalTransferResponse>
    {
        public int WalletID { get; set; }
        public TransferDirection TransferDirection { get; set; }
        public string PairWalletName { get; set; }
        public string PairWalletAddress { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = InputFormat.Amount, ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = InputFormat.UnitPrice, ApplyFormatInEditMode = true)]
        public decimal UnitPriceInUSD { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = InputFormat.TransactionDateTime, ApplyFormatInEditMode = true)]
        public DateTime TransactionDateTime { get; set; }
    }
}
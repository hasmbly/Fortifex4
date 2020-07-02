using System;
using System.ComponentModel.DataAnnotations;
using Fortifex4.Shared.Common;
using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer
{
    public class CreateInternalTransferRequest : IRequest<CreateInternalTransferResponse>
    {
        public int FromPocketID { get; set; }
        public int ToPocketID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = InputFormat.Amount, ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = InputFormat.TransactionDateTime, ApplyFormatInEditMode = true)]
        public DateTimeOffset TransactionDateTime { get; set; }
    }
}
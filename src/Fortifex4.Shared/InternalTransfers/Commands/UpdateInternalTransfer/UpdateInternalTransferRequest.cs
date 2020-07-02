using System;
using System.ComponentModel.DataAnnotations;
using Fortifex4.Shared.Common;
using MediatR;

namespace Fortifex4.Shared.InternalTransfers.Commands.UpdateInternalTransfer
{
    public class UpdateInternalTransferRequest : IRequest<UpdateInternalTransferResponse>
    {
        public int InternalTransferID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = InputFormat.Amount, ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = InputFormat.TransactionDateTime, ApplyFormatInEditMode = true)]
        public DateTimeOffset TransactionDateTime { get; set; }
    }
}
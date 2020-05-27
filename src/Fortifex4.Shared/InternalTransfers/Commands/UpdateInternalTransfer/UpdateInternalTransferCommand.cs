using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.InternalTransfers.Commands.UpdateInternalTransfer
{
    public class UpdateInternalTransferCommand : IRequest<UpdateInternalTransferResult>
    {
        public int InternalTransferID { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
    }

    public class UpdateInternalTransferCommandHandler : IRequestHandler<UpdateInternalTransferCommand, UpdateInternalTransferResult>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateInternalTransferCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateInternalTransferResult> Handle(UpdateInternalTransferCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateInternalTransferResult
            {
                IsSuccessful = false
            };

            var internalTransfer = await _context.InternalTransfers
                .Where(x => x.InternalTransferID == request.InternalTransferID)
                .SingleOrDefaultAsync(cancellationToken);

            if (internalTransfer == null)
                throw new NotFoundException(nameof(internalTransfer), request.InternalTransferID);

            decimal amount = request.Amount;

            var fromTransaction = await _context.Transactions
                .Where(x => x.TransactionID == internalTransfer.FromTransactionID)
                .SingleOrDefaultAsync(cancellationToken);

            fromTransaction.Amount = -amount;
            fromTransaction.TransactionDateTime = request.TransactionDateTime;

            var toTransaction = await _context.Transactions
                .Where(x => x.TransactionID == internalTransfer.ToTransactionID)
                .SingleOrDefaultAsync(cancellationToken);

            toTransaction.Amount = amount;
            toTransaction.TransactionDateTime = request.TransactionDateTime;

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
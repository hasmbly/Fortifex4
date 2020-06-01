using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer
{
    public class DeleteInternalTransferCommandHandler : IRequestHandler<DeleteInternalTransferRequest, DeleteInternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteInternalTransferCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteInternalTransferResponse> Handle(DeleteInternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteInternalTransferResponse
            {
                IsSuccessful = false
            };

            var internalTransfers = await _context.InternalTransfers
                .Where(x => x.InternalTransferID == request.InternalTransfersID)
                .SingleOrDefaultAsync();

            if (internalTransfers == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.InternalTransferNotFound;

                return result;
            }

            _context.InternalTransfers.Remove(internalTransfers);
            await _context.SaveChangesAsync(cancellationToken);

            var fromTransaction = await _context.Transactions
                .Where(x => x.TransactionID == internalTransfers.FromTransactionID)
                .SingleOrDefaultAsync();

            _context.Transactions.Remove(fromTransaction);
            await _context.SaveChangesAsync(cancellationToken);

            var toTransaction = await _context.Transactions
                .Where(x => x.TransactionID == internalTransfers.ToTransactionID)
                .SingleOrDefaultAsync();

            _context.Transactions.Remove(toTransaction);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
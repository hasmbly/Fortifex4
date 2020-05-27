using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.DeleteExternalTransfer
{
    public class DeleteExternalTransferCommand : IRequest<DeleteExternalTransferResult>
    {
        public int TransactionID { get; set; }
    }

    public class DeleteExternalTransferCommandHandler : IRequestHandler<DeleteExternalTransferCommand, DeleteExternalTransferResult>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteExternalTransferCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteExternalTransferResult> Handle(DeleteExternalTransferCommand request, CancellationToken cancellationToken)
        {
            var result = new DeleteExternalTransferResult();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;
            result.WalletID = transaction.Pocket.WalletID;

            return result;
        }
    }
}
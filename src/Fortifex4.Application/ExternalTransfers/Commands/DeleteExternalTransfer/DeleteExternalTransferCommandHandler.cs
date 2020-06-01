using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Wallets.Commands.DeleteExternalTransfer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.DeleteExternalTransfer
{
    public class DeleteExternalTransferCommandHandler : IRequestHandler<DeleteExternalTransferRequest, DeleteExternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteExternalTransferCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteExternalTransferResponse> Handle(DeleteExternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteExternalTransferResponse();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.TransactionNotFound;

                return result;
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;
            result.WalletID = transaction.Pocket.WalletID;

            return result;
        }
    }
}
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Fortifex4.Application.Sync.Commands.UpdateSync
{
    public class UpdateSyncCommandHandler : IRequestHandler<UpdateSyncCommand, UpdateSyncResult>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateSyncCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateSyncResult> Handle(UpdateSyncCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateSyncResult();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            transaction.UnitPriceInUSD = request.UnitPriceInUSD;

            if (transaction.TransactionType == TransactionType.SyncTransactionIN || transaction.TransactionType == TransactionType.SyncTransactionOUT)
            {
                transaction.PairWalletName = request.PairWalletName;
            }

            await _context.SaveChangesAsync(cancellationToken);

            result.WalletID = transaction.Pocket.WalletID;
            result.IsSuccessful = true;

            return result;
        }
    }
}
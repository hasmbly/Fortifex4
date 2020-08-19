using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Sync.Commands.UpdateSync;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Sync.Commands.UpdateSync
{
    public class UpdateSyncCommandHandler : IRequestHandler<UpdateSyncRequest, UpdateSyncResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateSyncCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateSyncResponse> Handle(UpdateSyncRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateSyncResponse();

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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Withdrawals.Commands.UpdateWithdrawal
{
    public class UpdateWithdrawalCommandHandler : IRequestHandler<UpdateWithdrawalCommand, UpdateWithdrawalResult>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateWithdrawalCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateWithdrawalResult> Handle(UpdateWithdrawalCommand request, CancellationToken cancellationToken)
        {
            UpdateWithdrawalResult result = new UpdateWithdrawalResult();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            result.WalletID = transaction.Pocket.WalletID;

            transaction.Amount = -request.Amount;
            transaction.TransactionDateTime = request.TransactionDateTime;

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
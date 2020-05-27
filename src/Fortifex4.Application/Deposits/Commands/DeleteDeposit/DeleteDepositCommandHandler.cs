using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Deposits.Commands.DeleteDeposit
{
    public class DeleteDepositCommandHandler : IRequestHandler<DeleteDepositCommand, DeleteDepositResult>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteDepositCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteDepositResult> Handle(DeleteDepositCommand request, CancellationToken cancellationToken)
        {
            DeleteDepositResult result = new DeleteDepositResult();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            result.WalletID = transaction.Pocket.WalletID;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
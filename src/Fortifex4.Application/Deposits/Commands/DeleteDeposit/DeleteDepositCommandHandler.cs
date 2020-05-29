using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Deposits.Commands.DeleteDeposit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Deposits.Commands.DeleteDeposit
{
    public class DeleteDepositCommandHandler : IRequestHandler<DeleteDepositRequest, DeleteDepositResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteDepositCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteDepositResponse> Handle(DeleteDepositRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteDepositResponse();

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
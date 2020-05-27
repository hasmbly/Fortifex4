using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Withdrawals.Commands.DeleteWithdrawal
{
    public class DeleteWithdrawalCommandHandler : IRequestHandler<DeleteWithdrawalCommand, DeleteWithdrawalResult>
    {
        private readonly IFortifex4DBContext _context;
        public DeleteWithdrawalCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }
        public async Task<DeleteWithdrawalResult> Handle(DeleteWithdrawalCommand request, CancellationToken cancellationToken)
        {
            DeleteWithdrawalResult result = new DeleteWithdrawalResult();

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

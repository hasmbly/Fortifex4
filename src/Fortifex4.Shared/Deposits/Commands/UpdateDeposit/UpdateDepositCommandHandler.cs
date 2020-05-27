using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Deposits.Commands.UpdateDeposit
{
    public class UpdateDepositCommandHandler : IRequestHandler<UpdateDepositCommand, UpdateDepositResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public UpdateDepositCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<UpdateDepositResult> Handle(UpdateDepositCommand request, CancellationToken cancellationToken)
        {
            UpdateDepositResult result = new UpdateDepositResult();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(a => a.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if(transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            transaction.Amount = request.Amount;
            transaction.TransactionDateTime = request.TransactionDateTime;
            transaction.LastModified = _dateTimeOffset.Now;

            await _context.SaveChangesAsync(cancellationToken);

            result.WalletID = transaction.Pocket.WalletID;
            result.IsSuccessful = true;

            return result;
        }
    }
}
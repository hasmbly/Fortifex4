using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Deposits.Commands.UpdateDeposit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Deposits.Commands.UpdateDeposit
{
    public class UpdateDepositCommandHandler : IRequestHandler<UpdateDepositRequest, UpdateDepositResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public UpdateDepositCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<UpdateDepositResponse> Handle(UpdateDepositRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateDepositResponse();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(a => a.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.TransactionNotFound;

                return result;
            }

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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.StartingBalance.Commands.UpdateStartingBalance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.StartingBalance.Commands.UpdateStartingBalance
{
    public class UpdateStartingBalanceCommandHandler : IRequestHandler<UpdateStartingBalanceRequest, UpdateStartingBalanceResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public UpdateStartingBalanceCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<UpdateStartingBalanceResponse> Handle(UpdateStartingBalanceRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateStartingBalanceResponse();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            transaction.Amount = request.Amount;
            transaction.UnitPriceInUSD = request.UnitPriceInUSD;
            transaction.LastModified = _dateTimeOffset.Now;

            await _context.SaveChangesAsync(cancellationToken);

            result.WalletID = transaction.Pocket.WalletID;
            result.IsSuccessful = true;

            return result;
        }
    }    
}
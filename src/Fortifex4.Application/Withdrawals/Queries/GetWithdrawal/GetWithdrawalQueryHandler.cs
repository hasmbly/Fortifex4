using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Withdrawals.Queries.GetWithdrawal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Withdrawals.Queries.GetWithdrawal
{
    public class GetWithdrawalQueryHandler : IRequestHandler<GetWithdrawalRequest, GetWithdrawalResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetWithdrawalQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetWithdrawalResponse> Handle(GetWithdrawalRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            var result = new GetWithdrawalResponse
            {
                Amount = transaction.Amount,
                TransactionDateTime = transaction.TransactionDateTime
            };

            return result;
        }
    }
}
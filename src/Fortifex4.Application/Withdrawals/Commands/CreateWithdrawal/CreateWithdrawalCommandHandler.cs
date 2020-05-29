using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Withdrawals.Commands.CreateWithdrawal;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Withdrawals.Commands.CreateWithdrawal
{
    public class CreateWithdrawalCommandHandler : IRequestHandler<CreateWithdrawalRequest, CreateWithdrawalResponse>
    {
        private readonly IFortifex4DBContext _context;

        public CreateWithdrawalCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CreateWithdrawalResponse> Handle(CreateWithdrawalRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateWithdrawalResponse();

            Wallet walletForWithdrawal = await _context.Wallets
                .Where(x => x.WalletID == request.WalletID)
                .Include(x => x.Pockets)
                .SingleOrDefaultAsync(cancellationToken);

            if (walletForWithdrawal == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            Pocket pocketForWithdrawal = walletForWithdrawal.Pockets.Single(x => x.IsMain);

            Transaction transactionForWithdrawal = new Transaction
            {
                PocketID = pocketForWithdrawal.PocketID,
                Amount = -request.Amount,
                TransactionHash = string.Empty,
                TransactionType = TransactionType.Withdrawal,
                TransactionDateTime = request.TransactionDateTime
            };

            _context.Transactions.Add(transactionForWithdrawal);
            await _context.SaveChangesAsync(cancellationToken);

            result.TransactionID = transactionForWithdrawal.TransactionID;
            result.WalletID = walletForWithdrawal.WalletID;

            return result;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.ExternalTransfers.Commands.UpdateExternalTransfer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.ExternalTransfers.Commands.UpdateExternalTransfer
{
    public class UpdateExternalTransferCommandHandler : IRequestHandler<UpdateExternalTransferRequest, UpdateExternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public UpdateExternalTransferCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<UpdateExternalTransferResponse> Handle(UpdateExternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateExternalTransferResponse();

            var transaction = await _context.Transactions
                .Where(x => x.TransactionID == request.TransactionID)
                .Include(y => y.Pocket)
                .SingleOrDefaultAsync(cancellationToken);

            if (transaction == null)
                throw new NotFoundException(nameof(Transaction), request.TransactionID);

            TransactionType transactionType = TransactionType.ExternalTransferIN;
            decimal amount = request.Amount;

            if (request.TransferDirection == TransferDirection.OUT)
            {
                transactionType = TransactionType.ExternalTransferOUT;
                amount = -request.Amount;
            }

            transaction.PairWalletName = string.IsNullOrEmpty(request.PairWalletName) ? "External Wallet" : request.PairWalletName;
            transaction.PairWalletAddress = string.IsNullOrEmpty(request.PairWalletAddress) ? "External Wallet Address" : request.PairWalletAddress;

            transaction.TransactionType = transactionType;
            transaction.Amount = amount;
            transaction.UnitPriceInUSD = request.UnitPriceInUSD;
            transaction.TransactionDateTime = request.TransactionDateTime;
            transaction.LastModified = _dateTimeOffset.Now;

            await _context.SaveChangesAsync(cancellationToken);

            result.WalletID = transaction.Pocket.WalletID;
            result.IsSuccessful = true;

            return result;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.CreateExternalTransfer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.CreateExternalTransfer
{
    public class CreateExternalTransferCommandHandler : IRequestHandler<CreateExternalTransferRequest, CreateExternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreateExternalTransferCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<CreateExternalTransferResponse> Handle(CreateExternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateExternalTransferResponse();

            Wallet wallet = await _context.Wallets
                .Where(x => x.WalletID == request.WalletID)
                .Include(x => x.Pockets)
                .SingleOrDefaultAsync(cancellationToken);

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            Pocket mainPocket = wallet.Pockets.Single(x => x.IsMain);

            decimal amount = request.Amount;
            TransactionType transactionType = TransactionType.ExternalTransferIN;

            if (request.TransferDirection == TransferDirection.OUT)
            {
                amount = -request.Amount;
                transactionType = TransactionType.ExternalTransferOUT;
            }

            Transaction transactionForExternalTransfer = new Transaction
            {
                PocketID = mainPocket.PocketID,
                Amount = amount,
                UnitPriceInUSD = request.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = string.IsNullOrEmpty(request.PairWalletName) ? "External Wallet" : request.PairWalletName,
                PairWalletAddress = string.IsNullOrEmpty(request.PairWalletAddress) ? "External Wallet Address" : request.PairWalletAddress,
                TransactionType = transactionType,
                TransactionDateTime = request.TransactionDateTime,
                Created = _dateTimeOffset.Now,
                LastModified = _dateTimeOffset.Now
            };

            _context.Transactions.Add(transactionForExternalTransfer);
            await _context.SaveChangesAsync(cancellationToken);

            result.TransactionID = transactionForExternalTransfer.TransactionID;

            return result;
        }
    }
}
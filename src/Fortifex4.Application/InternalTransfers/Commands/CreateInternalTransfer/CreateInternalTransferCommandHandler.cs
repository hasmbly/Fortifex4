using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.InternalTransfers.Commands.CreateInternalTransfer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.InternalTransfers.Commands.CreateInternalTransfer
{
    public class CreateInternalTransferCommandHandler : IRequestHandler<CreateInternalTransferRequest, CreateInternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreateInternalTransferCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<CreateInternalTransferResponse> Handle(CreateInternalTransferRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateInternalTransferResponse();

            var fromPocket = _context.Pockets
                .Where(x => x.PocketID == request.FromPocketID)
                .Include(a => a.Wallet)
                    .ThenInclude(b => b.Owner)
                    .ThenInclude(c => c.Provider)
                .SingleOrDefault();

            if (fromPocket == null)
                throw new NotFoundException(nameof(Pocket), request.FromPocketID);

            var toPocket = _context.Pockets
                .Where(x => x.PocketID == request.ToPocketID)
                .Include(a => a.Wallet)
                    .ThenInclude(b => b.Owner)
                    .ThenInclude(c => c.Provider)
                .SingleOrDefault();

            if (toPocket == null)
                throw new NotFoundException(nameof(Pocket), request.ToPocketID);

            var currency = _context.Currencies
                .Where(x => x.CurrencyID == fromPocket.CurrencyID)
                .SingleOrDefault();

            if (currency == null)
                throw new NotFoundException(nameof(Currency), fromPocket.CurrencyID);

            Transaction transactionFromPocket = new Transaction
            {
                PocketID = fromPocket.PocketID,
                Amount = -request.Amount,
                UnitPriceInUSD = currency.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = $"{toPocket.Wallet.Owner.Provider.Name} - {toPocket.Wallet.Name}",
                PairWalletAddress = toPocket.Wallet.Address,
                TransactionType = TransactionType.InternalTransferOUT,
                TransactionDateTime = request.TransactionDateTime,
                Created = _dateTimeOffset.Now,
                LastModified = _dateTimeOffset.Now
            };

            Transaction transactionToPocket = new Transaction
            {
                PocketID = toPocket.PocketID,
                Amount = request.Amount,
                UnitPriceInUSD = currency.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = $"{fromPocket.Wallet.Owner.Provider.Name} - {fromPocket.Wallet.Name}",
                PairWalletAddress = fromPocket.Wallet.Address,
                TransactionType = TransactionType.InternalTransferIN,
                TransactionDateTime = request.TransactionDateTime,
                Created = _dateTimeOffset.Now,
                LastModified = _dateTimeOffset.Now
            };

            _context.Transactions.Add(transactionFromPocket);
            _context.Transactions.Add(transactionToPocket);

            await _context.SaveChangesAsync(cancellationToken);

            InternalTransfer internalTransfer = new InternalTransfer
            {
                FromTransactionID = transactionFromPocket.TransactionID,
                ToTransactionID = transactionToPocket.TransactionID,
            };

            _context.InternalTransfers.Add(internalTransfer);

            await _context.SaveChangesAsync(cancellationToken);

            result.InternalTransferID = internalTransfer.InternalTransferID;
            result.WalletID = fromPocket.WalletID;

            return result;
        }
    }
}
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Deposits.Commands.CreateDeposit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Deposits.Commands.CreateDeposit
{
    public class CreateDepositCommandHandler : IRequestHandler<CreateDepositRequest, CreateDepositResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreateDepositCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<CreateDepositResponse> Handle(CreateDepositRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateDepositResponse();

            Wallet walletForDeposit = null;
            Pocket pocketForDeposit = null;

            if (request.WalletID.HasValue)
            {
                walletForDeposit = await _context.Wallets
                    .Where(x => x.WalletID == request.WalletID.Value)
                    .Include(x => x.Pockets)
                    .SingleOrDefaultAsync(cancellationToken);

                if (walletForDeposit == null)
                    throw new NotFoundException(nameof(Wallet), request.WalletID.Value);

                pocketForDeposit = walletForDeposit.Pockets.Single(x => x.IsMain);
            }
            else
            {
                Owner owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                .Include(a => a.Wallets).ThenInclude(x => x.Pockets)
                .SingleOrDefaultAsync(cancellationToken);

                if (owner == null)
                    throw new NotFoundException(nameof(Owner), request.OwnerID);

                Currency currency = await _context.Currencies
                    .Where(x => x.CurrencyID == request.CurrencyID)
                    .SingleOrDefaultAsync(cancellationToken);

                if (currency == null)
                    throw new NotFoundException(nameof(Currency), request.CurrencyID);

                #region Preparing Wallet and Pocket

                foreach (var wallet in owner.Wallets)
                {
                    foreach (var pocket in wallet.Pockets)
                    {
                        if (pocket.CurrencyID == request.CurrencyID)
                        {
                            pocketForDeposit = pocket;
                            walletForDeposit = wallet;
                            break;
                        }
                    }

                    if (walletForDeposit != null)
                        break;
                }

                if (pocketForDeposit == null)
                {
                    walletForDeposit = new Wallet
                    {
                        OwnerID = owner.OwnerID,
                        BlockchainID = currency.BlockchainID,
                        Name = currency.Name,
                        Address = string.Empty,
                        ProviderType = ProviderType.Exchange
                    };

                    _context.Wallets.Add(walletForDeposit);
                    await _context.SaveChangesAsync(cancellationToken);

                    pocketForDeposit = new Pocket
                    {
                        WalletID = walletForDeposit.WalletID,
                        CurrencyID = currency.CurrencyID,
                        CurrencyType = currency.CurrencyType,
                        Address = string.Empty,
                        IsMain = true
                    };

                    _context.Pockets.Add(pocketForDeposit);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                #endregion
            }

            Transaction transactionForDeposit = new Transaction
            {
                PocketID = pocketForDeposit.PocketID,
                Amount = request.Amount,
                TransactionHash = string.Empty,
                TransactionType = TransactionType.Deposit,
                TransactionDateTime = request.TransactionDateTime,
                Created = _dateTimeOffset.Now,
                LastModified = _dateTimeOffset.Now
            };

            _context.Transactions.Add(transactionForDeposit);
            await _context.SaveChangesAsync(cancellationToken);

            result.TransactionID = transactionForDeposit.TransactionID;
            result.PocketID = pocketForDeposit.PocketID;
            result.WalletID = walletForDeposit.WalletID;

            return result;
        }
    }
}
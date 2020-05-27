using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Application.Wallets.Commands.CreateExchangeWallet
{
    public class CreateExchangeWalletCommand : IRequest<int>
    {
        public int OwnerID { get; set; }
        public int CurrencyID { get; set; }
        public decimal? StartingBalance { get; set; }
    }

    public class CreateExchangeWalletCommandHandler : IRequestHandler<CreateExchangeWalletCommand, int>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreateExchangeWalletCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<int> Handle(CreateExchangeWalletCommand request, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners.FindAsync(request.OwnerID);

            if (owner == null)
                throw new NotFoundException(nameof(owner), request.OwnerID);

            var currency = await _context.Currencies.FindAsync(request.CurrencyID);

            if (currency == null)
                throw new NotFoundException(nameof(Currency), request.CurrencyID);

            var wallet = new Wallet
            {
                OwnerID = owner.OwnerID,
                BlockchainID = currency.BlockchainID,
                Name = currency.Name,
                Address = string.Empty,
                IsSynchronized = false,
                ProviderType = ProviderType.Exchange
            };

            _context.Wallets.Add(wallet);

            var pocket = new Pocket
            {
                CurrencyID = currency.CurrencyID,
                CurrencyType = currency.CurrencyType,
                Address = string.Empty,
                IsMain = true
            };

            wallet.Pockets.Add(pocket);

            if (request.StartingBalance.HasValue)
            {
                if (request.StartingBalance.Value > 0)
                {
                    Transaction transactionForStartingBalance = new Transaction
                    {
                        Amount = request.StartingBalance.Value,
                        UnitPriceInUSD = currency.UnitPriceInUSD,
                        TransactionHash = string.Empty,
                        PairWalletName = currency.Name,
                        PairWalletAddress = string.Empty,
                        TransactionType = TransactionType.StartingBalance,
                        TransactionDateTime = _dateTimeOffset.Now
                    };

                    pocket.Transactions.Add(transactionForStartingBalance);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return wallet.WalletID;
        }
    }
}
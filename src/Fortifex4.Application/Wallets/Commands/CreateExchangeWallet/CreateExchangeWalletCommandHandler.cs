using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.CreateExchangeWallet;
using MediatR;

namespace Fortifex4.Application.Wallets.Commands.CreateExchangeWallet
{
    public class CreateExchangeWalletCommandHandler : IRequestHandler<CreateExchangeWalletRequest, CreateExchangeWalletResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreateExchangeWalletCommandHandler(IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<CreateExchangeWalletResponse> Handle(CreateExchangeWalletRequest request, CancellationToken cancellationToken)
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

            decimal startingBalanceAmount = request.StartingBalance ?? 0m;

            var provider = await _context.Providers.FindAsync(owner.ProviderID);

            Transaction transactionForStartingBalance = new Transaction
            {
                Amount = startingBalanceAmount,
                UnitPriceInUSD = currency.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = $"{provider.Name} - {currency.Name}",
                PairWalletAddress = string.Empty,
                TransactionType = TransactionType.StartingBalance,
                TransactionDateTime = _dateTimeOffset.Now
            };

            pocket.Transactions.Add(transactionForStartingBalance);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateExchangeWalletResponse 
            {
                IsSuccessful = true,
                WalletID = wallet.WalletID
            };
        }
    }
}
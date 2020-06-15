using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fortifex4.Application.Wallets.Commands.CreatePersonalWallet
{
    public class CreatePersonalWalletCommandHandler : IRequestHandler<CreatePersonalWalletRequest, CreatePersonalWalletResponse>
    {
        private readonly ILogger<CreatePersonalWalletCommandHandler> _logger;
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;

        public CreatePersonalWalletCommandHandler(ILogger<CreatePersonalWalletCommandHandler> logger, IFortifex4DBContext context, IDateTimeOffsetService dateTimeOffset)
        {
            _logger = logger;
            _context = context;
            _dateTimeOffset = dateTimeOffset;
        }

        public async Task<CreatePersonalWalletResponse> Handle(CreatePersonalWalletRequest request, CancellationToken cancellationToken)
        {
            var blockchain = await _context.Blockchains.FindAsync(request.BlockchainID);

            if (blockchain == null)
                throw new NotFoundException(nameof(Blockchain), request.BlockchainID);

            _logger.LogDebug($"request.BlockchainID: {request.BlockchainID}");

            var coinCurrency = await _context.Currencies
                .Where(x =>
                    x.BlockchainID == request.BlockchainID &&
                    x.CurrencyType == CurrencyType.Coin)
                .SingleAsync(cancellationToken);

            var owner = await _context.Owners
                .Where(x =>
                    x.MemberUsername == request.MemberUsername &&
                    x.ProviderType == ProviderType.Personal)
                .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
            {
                owner = new Owner
                {
                    MemberUsername = request.MemberUsername,
                    ProviderType = ProviderType.Personal,
                    ProviderID = ProviderID.Personal
                };

                _context.Owners.Add(owner);
            }

            Wallet wallet = new Wallet
            {
                BlockchainID = request.BlockchainID,
                Name = request.Name,
                Address = request.Address,
                ProviderType = ProviderType.Personal
            };

            owner.Wallets.Add(wallet);

            Pocket pocket = new Pocket
            {
                CurrencyID = coinCurrency.CurrencyID,
                CurrencyType = CurrencyType.Coin,
                Address = request.Address,
                IsMain = true
            };

            wallet.Pockets.Add(pocket);

            decimal startingBalanceAmount = request.StartingBalance ?? 0m;

            Transaction transactionForStartingBalance = new Transaction
            {
                Amount = startingBalanceAmount,
                UnitPriceInUSD = coinCurrency.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = request.Name,
                PairWalletAddress = request.Address,
                TransactionType = TransactionType.StartingBalance,
                TransactionDateTime = _dateTimeOffset.Now
            };

            pocket.Transactions.Add(transactionForStartingBalance);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreatePersonalWalletResponse 
            {
                IsSuccessful = true,
                WalletID = wallet.WalletID
            };
        }
    }
}
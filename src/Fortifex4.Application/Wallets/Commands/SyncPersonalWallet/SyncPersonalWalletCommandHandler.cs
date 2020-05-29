using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Application.Common.Interfaces.Dogecoin;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Application.Wallets.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.SyncPersonalWallet;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.SyncPersonalWallet
{
    public class SyncPersonalWalletCommandHandler : IRequestHandler<SyncPersonalWalletRequest, SyncPersonalWalletResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;
        private readonly IBitcoinService _bitcoinService;
        private readonly IDogecoinService _dogecoinService;
        private readonly IEthereumService _ethereumService;
        private readonly IMediator _mediator;

        public SyncPersonalWalletCommandHandler(
            IFortifex4DBContext context,
            IDateTimeOffsetService dateTimeOffset,
            IBitcoinService bitcoinService,
            IDogecoinService dogecoinService,
            IEthereumService ethereumService,
            IMediator mediator)
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
            _bitcoinService = bitcoinService;
            _dogecoinService = dogecoinService;
            _ethereumService = ethereumService;
            _mediator = mediator;
        }

        public async Task<SyncPersonalWalletResponse> Handle(SyncPersonalWalletRequest request, CancellationToken cancellationToken)
        {
            var wallet = await _context.Wallets
                .Where(x => x.WalletID == request.WalletID)
                .Include(x => x.Blockchain)
                .SingleOrDefaultAsync(cancellationToken);

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), request.WalletID);

            if (wallet.Blockchain.Symbol == CurrencySymbol.BTC)
            {
                if (!wallet.IsSynchronized)
                {
                    var bitcoinWallet = await _bitcoinService.GetBitcoinWalletAsync(wallet.Address);
                    await EthereumSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, bitcoinWallet.Balance, cancellationToken);
                }
                else
                {
                    var transactionCollection = await _bitcoinService.GetBitcoinTransactionCollectionAsync(wallet.Address);

                    var mainPocket = _context.Pockets
                      .Where(x => x.WalletID == request.WalletID && x.IsMain)
                      .Include(a => a.Transactions)
                      .Single();

                    var lastTransactionDateTime = mainPocket.Transactions
                        .OrderByDescending(x => x.TransactionDateTime)
                        .Select(x => x.TransactionDateTime)
                        .First();

                    long lastTransactionTimeStamp = lastTransactionDateTime.ToUnixTimeSeconds();

                    var newTransactions = transactionCollection.Transactions
                        .Where(x => x.TimeStamp > lastTransactionTimeStamp)
                        .OrderBy(x => x.TimeStamp)
                        .ToList();

                    var currencyBTC = await _context.Currencies
                        .Where(x => x.Symbol == CurrencySymbol.BTC)
                        .SingleAsync(cancellationToken);

                    decimal unitPriceInUSD = currencyBTC.UnitPriceInUSD;

                    foreach (var transaction in newTransactions)
                    {
                        TransactionType transactionType = TransactionType.SyncTransactionIN;
                        decimal amount = transaction.Amount;
                        string pairWalletAddress = transaction.FromAddress;

                        if (transaction.FromAddress == wallet.Address)
                        {
                            transactionType = TransactionType.SyncTransactionOUT;
                            amount = -transaction.Amount;
                            pairWalletAddress = transaction.ToAddress;
                        }

                        _context.Transactions.Add(new Transaction
                        {
                            PocketID = mainPocket.PocketID,
                            TransactionHash = transaction.Hash,
                            PairWalletAddress = pairWalletAddress,
                            Amount = amount,
                            UnitPriceInUSD = unitPriceInUSD,
                            TransactionType = transactionType,
                            TransactionDateTime = DateTimeOffset.FromUnixTimeSeconds(transaction.TimeStamp)
                        });
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.DOGE)
            {
                if (!wallet.IsSynchronized)
                {
                    var dogecoinWallet = await _dogecoinService.GetDogecoinWalletAsync(wallet.Address);
                    await EthereumSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, dogecoinWallet.Balance, cancellationToken);
                }
            }
            else if (wallet.Blockchain.Symbol == CurrencySymbol.ETH)
            {
                var currencyETH = await _context.Currencies
                    .Where(x => x.Symbol == CurrencySymbol.ETH)
                    .SingleAsync(cancellationToken);

                decimal unitPriceInUSD = currencyETH.UnitPriceInUSD;

                if (!wallet.IsSynchronized)
                {
                    var ethereumWallet = await _ethereumService.GetEthereumWalletAsync(wallet.Address);
                    await EthereumSynchronizer.ImportBalance(_context, _dateTimeOffset.Now, wallet, ethereumWallet.Balance, cancellationToken);

                    /// 5) Repopulate the Token Pockets, including their own Balance Import Transaction
                    var validTokens = ethereumWallet.Tokens.Where(x => !string.IsNullOrEmpty(x.Name));

                    foreach (var token in validTokens)
                    {
                        /// Check if this is new Token Currency, based on its Symbol and Name property

                        var tokenCurrency = await _context.Currencies
                            .Where(x =>
                                x.Symbol == token.Symbol &&
                                x.Name == token.Name &&
                                x.CurrencyType == CurrencyType.Token)
                            .FirstOrDefaultAsync(cancellationToken);

                        /// If this is new Token Currency, add it to the Currencies table

                        if (tokenCurrency == null)
                        {
                            tokenCurrency = new Currency
                            {
                                BlockchainID = wallet.BlockchainID,
                                Symbol = token.Symbol,
                                Name = token.Name,
                                CurrencyType = CurrencyType.Token,
                                IsShownInTradePair = false,
                                IsForPreferredOption = false
                            };

                            _context.Currencies.Add(tokenCurrency);
                            await _context.SaveChangesAsync(cancellationToken);
                        }

                        Pocket tokenPocket = new Pocket
                        {
                            CurrencyID = tokenCurrency.CurrencyID,
                            CurrencyType = CurrencyType.Token,
                            Address = token.Address
                        };

                        wallet.Pockets.Add(tokenPocket);

                        tokenPocket.Transactions.Add(new Transaction
                        {
                            Amount = token.Balance,
                            UnitPriceInUSD = tokenCurrency.UnitPriceInUSD,
                            TransactionHash = string.Empty,
                            PairWalletName = wallet.Name,
                            PairWalletAddress = token.Address,
                            TransactionType = TransactionType.SyncBalanceImport,
                            TransactionDateTime = _dateTimeOffset.Now
                        });
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    var pockets = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID)
                            .Include(a => a.Transactions)
                         .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    var mainPocket = pockets.Single(x => x.IsMain);

                    await EthereumSynchronizer.ImportTransactions(_context, _ethereumService, mainPocket, cancellationToken);

                    var tokenPockets = pockets.Where(x => !x.IsMain);

                    foreach (var pocket in tokenPockets)
                    {
                        await EthereumSynchronizer.ImportTransactions(_context, _ethereumService, pocket, cancellationToken);
                    }
                }
            }

            return new SyncPersonalWalletResponse 
            {
                IsSuccessful = true
            };
        }
    }
}
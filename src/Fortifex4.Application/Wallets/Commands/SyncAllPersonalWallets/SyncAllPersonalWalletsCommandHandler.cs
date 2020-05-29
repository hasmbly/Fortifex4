using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Application.Wallets.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.SyncAllPersonalWallets;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.SyncAllPersonalWallets
{
    public class SyncAllPersonalWalletsCommandHandler : IRequestHandler<SyncAllPersonalWalletsRequest, SyncAllPersonalWalletsResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IDateTimeOffsetService _dateTimeOffset;
        private readonly IBitcoinService _bitcoinService;
        private readonly IEthereumService _ethereumService;

        public SyncAllPersonalWalletsCommandHandler
        (
            IFortifex4DBContext context,
            IDateTimeOffsetService dateTimeOffset,
            IBitcoinService bitcoinService,
            IEthereumService ethereumService
        )
        {
            _context = context;
            _dateTimeOffset = dateTimeOffset;
            _bitcoinService = bitcoinService;
            _ethereumService = ethereumService;
        }

        public async Task<SyncAllPersonalWalletsResponse> Handle(SyncAllPersonalWalletsRequest request, CancellationToken cancellationToken)
        {
            var result = new SyncAllPersonalWalletsResponse();

            var currencyETH = await _context.Currencies
                .Where(x => x.Symbol == CurrencySymbol.ETH && x.CurrencyType == CurrencyType.Coin)
                .SingleAsync(cancellationToken);

            var currencyBTC = await _context.Currencies
                .Where(x => x.Symbol == CurrencySymbol.BTC && x.CurrencyType == CurrencyType.Coin)
                .SingleAsync(cancellationToken);

            var unitPriceETHInUSD = currencyETH.UnitPriceInUSD;
            var unitPriceBTCInUSD = currencyBTC.UnitPriceInUSD;

            var synchronizedWallets = await _context.Wallets
                .Where(x => x.IsSynchronized)
                .Include(x => x.Blockchain)
                .ToListAsync(cancellationToken);

            for (int walletIndex = 0; walletIndex < synchronizedWallets.Count; walletIndex++)
            {
                var wallet = synchronizedWallets[walletIndex];

                WalletDTO walletDTO = new WalletDTO
                {
                    WalletID = wallet.WalletID,
                    Name = wallet.Name,
                    Address = wallet.Address,
                    BlockchainSymbol = wallet.Blockchain.Symbol
                };

                result.Wallets.Add(walletDTO);

                var mainPocket = _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.IsMain)
                        .Include(a => a.Currency)
                        .Include(a => a.Transactions)
                        .AsNoTracking()
                        .Single();

                PocketDTO mainPocketDTO = new PocketDTO
                {
                    PocketID = mainPocket.PocketID,
                    CurrencyID = mainPocket.CurrencyID,
                    CurrencySymbol = mainPocket.Currency.Symbol,
                    CurrencyName = mainPocket.Currency.Name
                };

                walletDTO.Pockets.Add(mainPocketDTO);

                var lastTransactionDateTime = mainPocket.Transactions
                    .OrderByDescending(x => x.TransactionDateTime)
                    .Select(x => x.TransactionDateTime)
                    .First();

                long lastTransactionTimeStamp = lastTransactionDateTime.ToUnixTimeSeconds();

                if (wallet.Blockchain.Symbol == CurrencySymbol.BTC)
                {
                    var bitcoinTransactionCollection = await _bitcoinService.GetBitcoinTransactionCollectionAsync(wallet.Address);

                    var newBitcoinTransactions = bitcoinTransactionCollection.Transactions
                        .Where(x => x.TimeStamp > lastTransactionTimeStamp)
                        .OrderBy(x => x.TimeStamp)
                        .ToList();

                    foreach (var bitcoinTransaction in newBitcoinTransactions)
                    {
                        TransactionType transactionType = TransactionType.SyncTransactionIN;
                        decimal amount = bitcoinTransaction.Amount;
                        string pairWalletAddress = bitcoinTransaction.FromAddress;

                        if (bitcoinTransaction.FromAddress == wallet.Address)
                        {
                            transactionType = TransactionType.SyncTransactionOUT;
                            amount = -bitcoinTransaction.Amount;
                            pairWalletAddress = bitcoinTransaction.ToAddress;
                        }

                        _context.Transactions.Add(new Transaction
                        {
                            PocketID = mainPocket.PocketID,
                            TransactionHash = bitcoinTransaction.Hash,
                            PairWalletAddress = pairWalletAddress,
                            Amount = amount,
                            UnitPriceInUSD = unitPriceBTCInUSD,
                            TransactionType = transactionType,
                            TransactionDateTime = DateTimeOffset.FromUnixTimeSeconds(bitcoinTransaction.TimeStamp),
                            Created = _dateTimeOffset.Now,
                            LastModified = _dateTimeOffset.Now
                        });
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
                else if (wallet.Blockchain.Symbol == CurrencySymbol.ETH)
                {
                    await EthereumSynchronizer.ImportTransactions(_context, _ethereumService, mainPocket, cancellationToken);

                    var tokenPockets = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && !x.IsMain)
                            .Include(a => a.Transactions)
                         .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    foreach (var pocket in tokenPockets)
                    {
                        await EthereumSynchronizer.ImportTransactions(_context, _ethereumService, pocket, cancellationToken);
                        await Task.Delay(1000);
                    }                    
                }

                await Task.Delay(1000);
            }

            return result;
        }
    }
}
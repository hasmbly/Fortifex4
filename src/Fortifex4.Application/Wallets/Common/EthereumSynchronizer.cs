using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Common
{
    public static class EthereumSynchronizer
    {
        public static async Task ImportBalance(IFortifex4DBContext _context, DateTimeOffset now, Wallet wallet, decimal importedBalance, CancellationToken cancellationToken)
        {
            var existingPockets = await _context.Pockets
                .Where(x => x.WalletID == wallet.WalletID)
                    .Include(a => a.Currency)
                    .Include(a => a.Transactions)
                        .ThenInclude(b => b.FromInternalTransfers)
                        .ThenInclude(c => c.ToTransaction)
                    .Include(a => a.Transactions)
                        .ThenInclude(b => b.ToInternalTransfers)
                        .ThenInclude(c => c.FromTransaction)
                .ToListAsync(cancellationToken);

            /// 1) Remove all Transactions
            foreach (Pocket existingPocket in existingPockets)
            {
                foreach (var transaction in existingPocket.Transactions)
                {
                    _context.InternalTransfers.RemoveRange(transaction.FromInternalTransfers);
                    _context.Transactions.RemoveRange(transaction.FromInternalTransfers.Select(x => x.ToTransaction));
                    _context.InternalTransfers.RemoveRange(transaction.ToInternalTransfers);
                    _context.Transactions.RemoveRange(transaction.ToInternalTransfers.Select(x => x.FromTransaction));
                }

                _context.Transactions.RemoveRange(existingPocket.Transactions);
            }

            /// 2) Remove all Pockets except the Main Pocket
            _context.Pockets.RemoveRange(existingPockets.Where(x => !x.IsMain));
            await _context.SaveChangesAsync(cancellationToken);

            var mainPocket = existingPockets.Single(x => x.IsMain);

            /// 3) Add Balance Import Transaction to the Main Pocket
            mainPocket.Transactions.Add(new Transaction
            {
                TransactionHash = string.Empty,
                Amount = importedBalance,
                UnitPriceInUSD = mainPocket.Currency.UnitPriceInUSD,
                TransactionType = TransactionType.SyncBalanceImport,
                PairWalletName = wallet.Name,
                PairWalletAddress = mainPocket.Address,
                TransactionDateTime = now
            });

            /// 4) Set Wallet's flag IsSynchronized to true
            wallet.IsSynchronized = true;
            await _context.SaveChangesAsync(cancellationToken);

            //=====================================
        }

        public static async Task ImportTransactions(IFortifex4DBContext _context, IEthereumService _ethereumService, Pocket pocket, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .Where(x => x.CurrencyID == pocket.CurrencyID)
                .SingleAsync(cancellationToken);

            decimal unitPriceInUSD = currency.UnitPriceInUSD;

            var transactionCollection = await _ethereumService.GetEthereumTransactionCollectionAsync(pocket.Address);

            var lastTransactionDateTime = pocket.Transactions
                .OrderByDescending(x => x.TransactionDateTime)
                .Select(x => x.TransactionDateTime)
                .First();

            long lastTransactionTimeStamp = lastTransactionDateTime.ToUnixTimeSeconds();

            var newTransactions = transactionCollection.Transactions
                .Where(x => x.TimeStamp > lastTransactionTimeStamp)
                .OrderBy(x => x.TimeStamp)
                .ToList();

            foreach (var transaction in newTransactions)
            {
                TransactionType transactionType = TransactionType.SyncTransactionIN;
                decimal amount = transaction.Amount;
                string pairWalletAddress = transaction.FromAddress;

                if (transaction.FromAddress == pocket.Address)
                {
                    transactionType = TransactionType.SyncTransactionOUT;
                    amount = -transaction.Amount;
                    pairWalletAddress = transaction.ToAddress;
                }

                _context.Transactions.Add(new Transaction
                {
                    PocketID = pocket.PocketID,
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
}

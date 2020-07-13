using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class GetTransactionsByMemberUsernameQueryHandler : IRequestHandler<GetTransactionsByMemberUsernameRequest, GetTransactionsByMemberUsernameResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetTransactionsByMemberUsernameQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetTransactionsByMemberUsernameResponse> Handle(GetTransactionsByMemberUsernameRequest query, CancellationToken cancellationToken)
        {
            List<TransactionDTO> transactions = new List<TransactionDTO>();

            var owners = await _context.Owners
                .Where(x => x.MemberUsername == query.MemberUsername)
                    .Include(a => a.Provider)
                    .Include(a => a.Wallets)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            foreach (var owner in owners)
            {
                foreach (var wallet in owner.Wallets)
                {
                    var pockets = await _context.Pockets
                        .Where(x => x.WalletID == wallet.WalletID && x.CurrencyType != CurrencyType.Fiat)
                            .Include(a => a.Currency)
                            .Include(a => a.Transactions)
                        .ToListAsync(cancellationToken);

                    foreach (var pocket in pockets)
                    {
                        foreach (var transaction in pocket.Transactions)
                        {
                            if (transaction.TransactionType == TransactionType.StartingBalance ||
                                transaction.TransactionType == TransactionType.ExternalTransferIN ||
                                transaction.TransactionType == TransactionType.ExternalTransferOUT ||
                                transaction.TransactionType == TransactionType.InternalTransferIN ||
                                transaction.TransactionType == TransactionType.InternalTransferOUT ||
                                transaction.TransactionType == TransactionType.BuyIN ||
                                transaction.TransactionType == TransactionType.BuyOUT ||
                                transaction.TransactionType == TransactionType.SellIN ||
                                transaction.TransactionType == TransactionType.SellOUT ||
                                transaction.TransactionType == TransactionType.SyncBalanceImport ||
                                transaction.TransactionType == TransactionType.SyncTransactionIN ||
                                transaction.TransactionType == TransactionType.SyncTransactionOUT)
                            {
                                TransactionDTO transactionDTO = new TransactionDTO
                                {
                                    TransactionID = transaction.TransactionID,
                                    SourceProviderName = owner.Provider.Name,
                                    DestinationProviderName = "Destination Provider Name",
                                    SourceWalletName = wallet.Name,
                                    DestinationWalletName = transaction.PairWalletName,
                                    SourceCurrencySymbol = pocket.Currency.Symbol,
                                    DestinationCurrencySymbol = "Destination Currency Symbol",
                                    SourceCurrencyName = pocket.Currency.Name,
                                    DestinationCurrencyName = "Destination Currency Name",
                                    Amount = Math.Abs(transaction.Amount),
                                    UnitPriceInUSD = transaction.UnitPriceInUSD,
                                    UnitPrice = 1,
                                    TransactionDateTime = transaction.TransactionDateTime,
                                    TransactionType = transaction.TransactionType,
                                    TransactionTypeDisplayText = transaction.TransactionTypeDisplayText,
                                    TradeID = null,
                                    InternalTransferID = null
                                };

                                if (transaction.TransactionType == TransactionType.StartingBalance)
                                {
                                    transactionDTO.SourceProviderName = wallet.Name;
                                    transactionDTO.DestinationProviderName = transaction.PairWalletName;
                                    transactionDTO.DestinationWalletName = transaction.PairWalletName;
                                    transactionDTO.DestinationCurrencySymbol = pocket.Currency.Symbol;
                                    transactionDTO.DestinationCurrencyName = pocket.Currency.Name;
                                }
                                else if (transaction.TransactionType == TransactionType.ExternalTransferIN ||
                                    transaction.TransactionType == TransactionType.ExternalTransferOUT)
                                {
                                    if (wallet.ProviderType == ProviderType.Personal)
                                    {
                                        transactionDTO.SourceProviderName = wallet.Name;
                                    }

                                    transactionDTO.DestinationProviderName = transaction.PairWalletName;
                                    transactionDTO.DestinationWalletName = transaction.PairWalletName;
                                    transactionDTO.DestinationCurrencySymbol = pocket.Currency.Symbol;
                                    transactionDTO.DestinationCurrencyName = pocket.Currency.Name;
                                    transactionDTO.TransactionID = transaction.TransactionID;
                                }
                                else if (transaction.TransactionType == TransactionType.InternalTransferIN)
                                {
                                    InternalTransfer internalTransfer = await _context.InternalTransfers
                                        .Where(x => x.ToTransactionID == transaction.TransactionID)
                                        .Include(a => a.FromTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                            .ThenInclude(c => c.Owner)
                                            .ThenInclude(c => c.Provider)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (internalTransfer != null)
                                    {
                                        if (wallet.ProviderType == ProviderType.Personal)
                                        {
                                            transactionDTO.SourceProviderName = wallet.Name;
                                        }

                                        if (internalTransfer.FromTransaction.Pocket.Wallet.Owner.Provider.ProviderType == ProviderType.Personal)
                                        {
                                            transactionDTO.DestinationProviderName = internalTransfer.FromTransaction.Pocket.Wallet.Name;
                                        }
                                        else
                                        {
                                            transactionDTO.DestinationProviderName = internalTransfer.FromTransaction.Pocket.Wallet.Owner.Provider.Name;
                                        }

                                        transactionDTO.DestinationWalletName = internalTransfer.FromTransaction.Pocket.Wallet.Name;
                                        transactionDTO.DestinationCurrencySymbol = pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = pocket.Currency.Name;
                                        transactionDTO.InternalTransferID = internalTransfer.InternalTransferID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.InternalTransferOUT)
                                {
                                    InternalTransfer internalTransfer = await _context.InternalTransfers
                                        .Where(x => x.FromTransactionID == transaction.TransactionID)
                                        .Include(a => a.ToTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                            .ThenInclude(c => c.Owner)
                                            .ThenInclude(d => d.Provider)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (internalTransfer != null)
                                    {
                                        if (wallet.ProviderType == ProviderType.Personal)
                                        {
                                            transactionDTO.SourceProviderName = wallet.Name;
                                        }

                                        if (internalTransfer.ToTransaction.Pocket.Wallet.Owner.Provider.ProviderType == ProviderType.Personal)
                                        {
                                            transactionDTO.DestinationProviderName = internalTransfer.ToTransaction.Pocket.Wallet.Name;
                                        }
                                        else
                                        {
                                            transactionDTO.DestinationProviderName = internalTransfer.ToTransaction.Pocket.Wallet.Owner.Provider.Name;
                                        }

                                        transactionDTO.DestinationWalletName = internalTransfer.ToTransaction.Pocket.Wallet.Name;
                                        transactionDTO.DestinationCurrencySymbol = pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = pocket.Currency.Name;
                                        transactionDTO.InternalTransferID = internalTransfer.InternalTransferID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.BuyIN)
                                {
                                    Trade tradeBuy = await _context.Trades
                                        .Where(x => x.FromTransactionID == transaction.TransactionID)
                                        .Include(a => a.ToTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Currency)
                                        .Include(a => a.ToTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (tradeBuy != null)
                                    {
                                        transactionDTO.DestinationProviderName = owner.Provider.Name;
                                        transactionDTO.DestinationWalletName = tradeBuy.ToTransaction.Pocket.Wallet.Name;
                                        transactionDTO.UnitPrice = tradeBuy.UnitPrice;
                                        transactionDTO.DestinationCurrencySymbol = tradeBuy.ToTransaction.Pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = tradeBuy.ToTransaction.Pocket.Currency.Name;
                                        transactionDTO.TradeID = tradeBuy.TradeID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.BuyOUT)
                                {
                                    Trade tradeBuy = await _context.Trades
                                        .Where(x => x.ToTransactionID == transaction.TransactionID)
                                        .Include(a => a.FromTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Currency)
                                        .Include(a => a.FromTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (tradeBuy != null)
                                    {
                                        transactionDTO.DestinationProviderName = owner.Provider.Name;
                                        transactionDTO.DestinationWalletName = tradeBuy.FromTransaction.Pocket.Wallet.Name;
                                        transactionDTO.UnitPrice = tradeBuy.FromTransaction.Amount / ((transactionDTO.Amount == 0) ? 0 : transactionDTO.Amount);
                                        transactionDTO.DestinationCurrencySymbol = tradeBuy.FromTransaction.Pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = tradeBuy.FromTransaction.Pocket.Currency.Name;
                                        transactionDTO.TradeID = tradeBuy.TradeID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.SellIN)
                                {
                                    Trade tradeSell = await _context.Trades
                                        .Where(x => x.ToTransactionID == transaction.TransactionID)
                                        .Include(a => a.FromTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Currency)
                                        .Include(a => a.FromTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (tradeSell != null)
                                    {
                                        transactionDTO.DestinationProviderName = owner.Provider.Name;
                                        transactionDTO.DestinationWalletName = tradeSell.FromTransaction.Pocket.Wallet.Name;
                                        transactionDTO.UnitPrice = Math.Abs(tradeSell.FromTransaction.Amount / transactionDTO.Amount);
                                        transactionDTO.DestinationCurrencySymbol = tradeSell.FromTransaction.Pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = tradeSell.FromTransaction.Pocket.Currency.Name;
                                        transactionDTO.TradeID = tradeSell.TradeID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.SellOUT)
                                {
                                    Trade tradeSell = await _context.Trades
                                        .Where(x => x.FromTransactionID == transaction.TransactionID)
                                        .Include(a => a.ToTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Currency)
                                        .Include(a => a.ToTransaction)
                                            .ThenInclude(b => b.Pocket)
                                            .ThenInclude(c => c.Wallet)
                                        .SingleOrDefaultAsync(cancellationToken);

                                    if (tradeSell != null)
                                    {
                                        transactionDTO.DestinationProviderName = owner.Provider.Name;
                                        transactionDTO.DestinationWalletName = tradeSell.ToTransaction.Pocket.Wallet.Name;
                                        transactionDTO.UnitPrice = tradeSell.UnitPrice;
                                        transactionDTO.DestinationCurrencySymbol = tradeSell.ToTransaction.Pocket.Currency.Symbol;
                                        transactionDTO.DestinationCurrencyName = tradeSell.ToTransaction.Pocket.Currency.Name;
                                        transactionDTO.TradeID = tradeSell.TradeID;
                                    }
                                }
                                else if (transaction.TransactionType == TransactionType.SyncBalanceImport ||
                                    transaction.TransactionType == TransactionType.SyncTransactionIN ||
                                    transaction.TransactionType == TransactionType.SyncTransactionOUT)
                                {
                                    transactionDTO.SourceProviderName = wallet.Name;
                                    transactionDTO.DestinationProviderName = transaction.PairWalletName;
                                    transactionDTO.DestinationWalletName = transaction.PairWalletName;
                                    transactionDTO.DestinationCurrencySymbol = pocket.Currency.Symbol;
                                    transactionDTO.DestinationCurrencyName = pocket.Currency.Name;
                                }

                                transactions.Add(transactionDTO);
                            }
                        }
                    }
                }
            }

            var result = new GetTransactionsByMemberUsernameResponse
            {
                Transactions = transactions.OrderByDescending(x => x.TransactionDateTime).ToList()
            };

            return result;
        }
    }
}
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Wallets.Queries.GetWallet
{
    public class GetWalletQueryHandler : IRequestHandler<GetWalletRequest, GetWalletResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetWalletQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetWalletResponse> Handle(GetWalletRequest query, CancellationToken cancellationToken)
        {
            Wallet wallet = await _context.Wallets
                .Where(x => x.WalletID == query.WalletID)
                .Include(a => a.Blockchain)
                .Include(a => a.Pockets).ThenInclude(b => b.Currency)
                .Include(a => a.Pockets).ThenInclude(b => b.Transactions)
                .Include(a => a.Owner).ThenInclude(b => b.Provider)
                .SingleOrDefaultAsync(cancellationToken);

            if (wallet == null)
                throw new NotFoundException(nameof(Wallet), query.WalletID);

            var result = new GetWalletResponse
            {
                WalletID = wallet.WalletID,
                OwnerID = wallet.OwnerID,
                BlockchainID = wallet.BlockchainID,
                Name = wallet.Name,
                Address = wallet.Address,
                ProviderType = wallet.ProviderType,
                IsSynchronized = wallet.IsSynchronized,
                BlockchainName = wallet.Blockchain.Name,
                OwnerProviderName = wallet.Owner.Provider.Name
            };

            #region Process Main Pocket

            var mainPocket = wallet.Pockets.Single(x => x.IsMain);

            PocketDTO mainPocketDTO = new PocketDTO
            {
                PocketID = mainPocket.PocketID,
                WalletID = mainPocket.WalletID,
                CurrencyID = mainPocket.CurrencyID,
                CurrencyType = mainPocket.CurrencyType,
                Address = mainPocket.Address,
                Balance = mainPocket.Transactions.Sum(x => x.Amount),
                CurrencySymbol = mainPocket.Currency.Symbol,
                CurrencyName = mainPocket.Currency.Name,
                Transactions = new List<TransactionDTO>()
            };

            result.MainPocket = mainPocketDTO;

            foreach (var transaction in mainPocket.Transactions)
            {
                if (transaction.TransactionType != TransactionType.BuyOUTNonWithholding &&
                    transaction.TransactionType != TransactionType.SellINNonWithholding)
                {
                    TransactionDTO transactionDTO = new TransactionDTO
                    {
                        TransactionID = transaction.TransactionID,
                        PocketID = transaction.PocketID,
                        TransactionHash = transaction.TransactionHash,
                        PairWalletName = transaction.PairWalletName,
                        PairWalletAddress = transaction.PairWalletAddress,
                        Amount = transaction.Amount,
                        UnitPriceInUSD = transaction.UnitPriceInUSD,
                        TransactionType = transaction.TransactionType,
                        TransactionDateTime = transaction.TransactionDateTime,
                        TransactionTypeDisplayText = transaction.TransactionTypeDisplayText,
                        TradeID = null,
                        InternalTransferID = null
                    };

                    if (transaction.TransactionType == TransactionType.InternalTransferOUT)
                    {
                        InternalTransfer internalTransfer = await _context.InternalTransfers
                            .Where(x => x.FromTransactionID == transaction.TransactionID)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (internalTransfer != null)
                        {
                            transactionDTO.InternalTransferID = internalTransfer.InternalTransferID;
                        }
                    }
                    else if (transaction.TransactionType == TransactionType.InternalTransferIN)
                    {
                        InternalTransfer internalTransfer = await _context.InternalTransfers
                            .Where(x => x.ToTransactionID == transaction.TransactionID)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (internalTransfer != null)
                        {
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
                            .SingleOrDefaultAsync(cancellationToken);

                        if (tradeBuy != null)
                        {
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
                            .SingleOrDefaultAsync(cancellationToken);

                        if (tradeBuy != null)
                        {
                            transactionDTO.TradeID = tradeBuy.TradeID;
                        }
                    }
                    else if (transaction.TransactionType == TransactionType.SellOUT)
                    {
                        Trade tradeSell = await _context.Trades
                            .Where(x => x.FromTransactionID == transaction.TransactionID)
                            .Include(a => a.ToTransaction)
                                .ThenInclude(b => b.Pocket)
                                .ThenInclude(c => c.Currency)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (tradeSell != null)
                        {
                            transactionDTO.TradeID = tradeSell.TradeID;
                        }
                    }
                    else if (transaction.TransactionType == TransactionType.SellIN)
                    {
                        Trade tradeSell = await _context.Trades
                            .Where(x => x.ToTransactionID == transaction.TransactionID)
                            .Include(a => a.FromTransaction)
                                .ThenInclude(b => b.Pocket)
                                .ThenInclude(c => c.Currency)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (tradeSell != null)
                        {
                            transactionDTO.TradeID = tradeSell.TradeID;
                        }
                    }

                    mainPocketDTO.Transactions.Add(transactionDTO);
                }
            }

            #endregion

            #region Process Token Pockets

            foreach (Pocket pocket in wallet.Pockets.Where(x => !x.IsMain))
            {
                PocketDTO tokenPocketDTO = new PocketDTO
                {
                    PocketID = pocket.PocketID,
                    WalletID = pocket.WalletID,
                    CurrencyID = pocket.CurrencyID,
                    CurrencyType = pocket.CurrencyType,
                    Address = pocket.Address,
                    Balance = pocket.Transactions.Sum(x => x.Amount),
                    CurrencySymbol = pocket.Currency.Symbol,
                    CurrencyName = pocket.Currency.Name
                };

                foreach (Transaction transaction in pocket.Transactions)
                {
                    tokenPocketDTO.Transactions.Add(new TransactionDTO
                    {
                        TransactionID = transaction.TransactionID,
                        PocketID = transaction.PocketID,
                        TransactionHash = transaction.TransactionHash,
                        PairWalletName = transaction.PairWalletName,
                        PairWalletAddress = transaction.PairWalletAddress,
                        Amount = transaction.Amount,
                        UnitPriceInUSD = transaction.UnitPriceInUSD,
                        TransactionType = transaction.TransactionType,
                        TransactionDateTime = transaction.TransactionDateTime,
                        TransactionTypeDisplayText = transaction.TransactionTypeDisplayText,
                        TradeID = null,
                        InternalTransferID = null
                    });
                }

                result.TokenPockets.Add(tokenPocketDTO);
            };

            #endregion

            return result;
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Trades.Commands.CreateTrade
{
    public class CreateTradeCommand : IRequest<CreateTradeResult>
    {
        public int OwnerID { get; set; }
        public int FromCurrencyID { get; set; }
        public int ToCurrencyID { get; set; }
        public decimal Amount { get; set; }
        public TradeType TradeType { get; set; }
        public DateTimeOffset TransactionDateTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInUSD { get; set; }
        public bool IsWithholding { get; set; }
    }

    public class CreateTradeCommandHandler : IRequestHandler<CreateTradeCommand, CreateTradeResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly ICryptoService _cryptoService;

        public CreateTradeCommandHandler(IFortifex4DBContext context, ICryptoService cryptoService)
        {
            _context = context;
            _cryptoService = cryptoService;
        }

        public async Task<CreateTradeResult> Handle(CreateTradeCommand request, CancellationToken cancellationToken)
        {
            var result = new CreateTradeResult();

            Owner owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                .Include(a => a.Provider)
                .Include(a => a.Wallets).ThenInclude(x => x.Pockets)
                .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
                throw new NotFoundException(nameof(Owner), request.OwnerID);

            Currency fromCurrency = await _context.Currencies
                .Where(x => x.CurrencyID == request.FromCurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (fromCurrency == null)
                throw new NotFoundException(nameof(Currency), request.FromCurrencyID);

            Currency toCurrency = await _context.Currencies
                .Where(x => x.CurrencyID == request.ToCurrencyID)
                .SingleOrDefaultAsync(cancellationToken);

            if (toCurrency == null)
                throw new NotFoundException(nameof(Currency), request.ToCurrencyID);

            #region Preparing Wallets and Pockets
            Pocket fromPocket = null;
            Pocket toPocket = null;

            foreach (var wallet in owner.Wallets)
            {
                foreach (var pocket in wallet.Pockets)
                {
                    if (pocket.CurrencyID == request.FromCurrencyID)
                        fromPocket = pocket;

                    if (pocket.CurrencyID == request.ToCurrencyID)
                        toPocket = pocket;

                    if (fromPocket != null && toPocket != null)
                        break;
                }

                if (fromPocket != null && toPocket != null)
                    break;
            }

            if (fromPocket == null)
            {
                Wallet fromWallet = new Wallet
                {
                    OwnerID = owner.OwnerID,
                    BlockchainID = fromCurrency.BlockchainID,
                    Name = fromCurrency.Name,
                    Address = string.Empty,
                    ProviderType = ProviderType.Exchange
                };

                _context.Wallets.Add(fromWallet);
                await _context.SaveChangesAsync(cancellationToken);

                fromPocket = new Pocket
                {
                    WalletID = fromWallet.WalletID,
                    CurrencyID = fromCurrency.CurrencyID,
                    CurrencyType = fromCurrency.CurrencyType,
                    Address = string.Empty,
                    IsMain = true
                };

                _context.Pockets.Add(fromPocket);
                await _context.SaveChangesAsync(cancellationToken);
            }

            if (toPocket == null)
            {
                Wallet toWallet = new Wallet
                {
                    OwnerID = owner.OwnerID,
                    BlockchainID = toCurrency.BlockchainID,
                    Name = toCurrency.Name,
                    Address = string.Empty,
                    ProviderType = ProviderType.Exchange
                };

                _context.Wallets.Add(toWallet);
                await _context.SaveChangesAsync(cancellationToken);

                toPocket = new Pocket
                {
                    WalletID = toWallet.WalletID,
                    CurrencyID = toCurrency.CurrencyID,
                    CurrencyType = toCurrency.CurrencyType,
                    Address = string.Empty,
                    IsMain = true
                };

                _context.Pockets.Add(toPocket);
                await _context.SaveChangesAsync(cancellationToken);
            }
            #endregion

            #region Create FromTransaction and ToTransaction
            Transaction fromTransaction = new Transaction
            {
                PocketID = fromPocket.PocketID,
                UnitPriceInUSD = request.UnitPriceInUSD,
                TransactionHash = string.Empty,
                PairWalletName = $"{owner.Provider.Name} - {toCurrency.Name}",
                PairWalletAddress = string.Empty,
                TransactionDateTime = request.TransactionDateTime
            };

            decimal unitPriceInUSDForToTransaction = await _cryptoService.GetUnitPriceAsync(toCurrency.Symbol, CurrencySymbol.USD);            

            Transaction toTransaction = new Transaction
            {
                PocketID = toPocket.PocketID,
                UnitPriceInUSD = unitPriceInUSDForToTransaction,
                TransactionHash = string.Empty,
                PairWalletName = $"{owner.Provider.Name} - {fromCurrency.Name}",
                PairWalletAddress = string.Empty,
                TransactionDateTime = request.TransactionDateTime
            };

            decimal unitPrice = decimal.Round(request.UnitPrice, 4);
            decimal totalPrice = request.Amount * unitPrice;

            if (request.TradeType == TradeType.Buy)
            {
                fromTransaction.Amount = request.Amount;
                fromTransaction.TransactionType = TransactionType.BuyIN;

                if (request.IsWithholding)
                {
                    toTransaction.Amount = -totalPrice;
                    toTransaction.TransactionType = TransactionType.BuyOUT;
                }
                else
                {
                    toTransaction.Amount = 0m;
                    toTransaction.TransactionType = TransactionType.BuyOUTNonWithholding;
                }
            }
            else if (request.TradeType == TradeType.Sell)
            {
                fromTransaction.Amount = -request.Amount;
                fromTransaction.TransactionType = TransactionType.SellOUT;

                if (request.IsWithholding)
                {
                    toTransaction.Amount = totalPrice;
                    toTransaction.TransactionType = TransactionType.SellIN;

                }
                else
                {
                    toTransaction.Amount = 0m;
                    toTransaction.TransactionType = TransactionType.SellINNonWithholding;
                }
            }

            _context.Transactions.Add(fromTransaction);
            _context.Transactions.Add(toTransaction);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            #region Create Trade
            Trade trade= new Trade
            {
                TradeType = request.TradeType,
                UnitPrice = unitPrice,
                FromTransactionID = fromTransaction.TransactionID,
                ToTransactionID = toTransaction.TransactionID,
                IsWithholding = request.IsWithholding
            };

            _context.Trades.Add(trade);
            await _context.SaveChangesAsync(cancellationToken);
            #endregion

            result.TradeID = trade.TradeID;

            return result;
        }
    }
}
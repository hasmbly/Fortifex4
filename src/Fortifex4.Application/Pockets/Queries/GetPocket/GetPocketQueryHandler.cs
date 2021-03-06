﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared.Pockets.Queries.GetPocket;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Pockets.Queries.GetPocket
{
    public class GetPocketQueryHandler : IRequestHandler<GetPocketRequest, GetPocketResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetPocketQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetPocketResponse> Handle(GetPocketRequest query, CancellationToken cancellationToken)
        {
            var pocket = await _context.Pockets
                .Where(x => x.PocketID == query.PocketID)
                .Include(a => a.Currency)
                .Include(a => a.Wallet).ThenInclude(b => b.Blockchain)
                .Include(a => a.Transactions)
                .SingleOrDefaultAsync(cancellationToken);

            if (pocket == null)
                throw new NotFoundException(nameof(Pocket), query.PocketID);

            var result = new GetPocketResponse
            {
                IsSuccessful = true,

                PocketID = pocket.PocketID,
                WalletID = pocket.WalletID,
                Address = pocket.Address,

                CurrencySymbol = pocket.Currency.Symbol,
                CurrencyName = pocket.Currency.Name,
                WalletName = pocket.Wallet.Name,
                WalletBlockchainName = pocket.Wallet.Blockchain.Name,
            };

            foreach (Transaction transaction in pocket.Transactions)
            {
                result.Transactions.Add(new TransactionDTO
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
                    TransactionTypeDisplayText = transaction.TransactionTypeDisplayText
                });
            }

            return result;
        }
    }
}
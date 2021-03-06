﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Shared.InternalTransfers.Queries.GetInternalTransfer
{
    public class GetInternalTransferQueryHandler : IRequestHandler<GetInternalTransferRequest, GetInternalTransferResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetInternalTransferQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetInternalTransferResponse> Handle(GetInternalTransferRequest query, CancellationToken cancellationToken)
        {
            var internalTransfer = await _context.InternalTransfers
                .Where(x => x.InternalTransferID == query.InternalTransferID)
                .Include(a => a.FromTransaction)
                    .ThenInclude(b => b.Pocket)
                    .ThenInclude(c => c.Currency)
                .Include(a => a.FromTransaction)
                    .ThenInclude(b => b.Pocket)
                    .ThenInclude(c => c.Wallet)
                    .ThenInclude(d => d.Owner)
                    .ThenInclude(e => e.Provider)
                .Include(a => a.ToTransaction)
                    .ThenInclude(b => b.Pocket)
                    .ThenInclude(c => c.Wallet)
                    .ThenInclude(d => d.Owner)
                    .ThenInclude(e => e.Provider)
                .SingleOrDefaultAsync(cancellationToken);

            if (internalTransfer == null)
            {
                return new GetInternalTransferResponse
                {
                    IsSuccessful = false,
                    ErrorMessage = ErrorMessage.InternalTransferNotFound
                };
            }

            return new GetInternalTransferResponse
            {
                IsSuccessful = true,

                InternalTransferID = internalTransfer.InternalTransferID,
                TransactionAmount = Math.Abs(internalTransfer.FromTransaction.Amount),
                TransactionDateTime = internalTransfer.FromTransaction.TransactionDateTime,
                FromTransactionPocketID = internalTransfer.FromTransaction.Pocket.PocketID,
                FromTransactionPocketWalletID = internalTransfer.FromTransaction.Pocket.WalletID,

                FromWalletName = $"{internalTransfer.FromTransaction.Pocket.Wallet.Owner.Provider.Name} - {internalTransfer.FromTransaction.Pocket.Wallet.Name}",
                ToWalletName = $"{internalTransfer.ToTransaction.Pocket.Wallet.Owner.Provider.Name} - {internalTransfer.ToTransaction.Pocket.Wallet.Name}",
                CurrencyName = internalTransfer.FromTransaction.Pocket.Currency.Name
            };
        }
    }
}
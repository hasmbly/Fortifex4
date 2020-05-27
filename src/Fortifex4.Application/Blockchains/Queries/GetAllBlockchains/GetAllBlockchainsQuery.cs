using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Blockchains.Queries.GetAllBlockchains
{
    public class GetAllBlockchainsQuery : IRequest<GetAllBlockchainsResult>
    {
    }

    public class GetAllBlockchainsQueryHandler : IRequestHandler<GetAllBlockchainsQuery, GetAllBlockchainsResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllBlockchainsQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllBlockchainsResult> Handle(GetAllBlockchainsQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllBlockchainsResult();

            var blockchains = await _context.Blockchains
                .Where(x => x.BlockchainID != BlockchainID.Fiat)
                .OrderBy(x => x.Rank)
                .ToListAsync(cancellationToken);

            foreach (var blockchain in blockchains)
            {
                result.Blockchains.Add(new BlockchainDTO
                {
                    BlockchainID = blockchain.BlockchainID,
                    Symbol = blockchain.Symbol,
                    Name = blockchain.Name,
                    Rank = blockchain.Rank
                });
            }

            return result;
        }
    }
}
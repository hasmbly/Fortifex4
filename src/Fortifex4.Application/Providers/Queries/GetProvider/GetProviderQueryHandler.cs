using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Providers.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetProviderQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var provider = await _context.Providers
                .Where(x => x.ProviderID == request.ProviderID)
                .SingleOrDefaultAsync(cancellationToken);

            if (provider == null)
                throw new NotFoundException(nameof(Provider), request.ProviderID);

            var result = new GetProviderResult
            {
                ProviderID = provider.ProviderID,
                Name = provider.Name,
                ProviderType = provider.ProviderType,
                SiteURL = provider.SiteURL
            };

            return result;
        }
    }
}
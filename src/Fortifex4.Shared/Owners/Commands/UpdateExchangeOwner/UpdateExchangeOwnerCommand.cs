using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Owners.Commands.UpdateExchangeOwner
{
    public class UpdateExchangeOwnerCommand : IRequest
    {
        public int OwnerID { get; set; }
        public int ProviderID { get; set; }
    }

    public class UpdateExchangeOwnerCommandHandler : IRequestHandler<UpdateExchangeOwnerCommand>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateExchangeOwnerCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateExchangeOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
                throw new NotFoundException(nameof(Provider), request.ProviderID);

            owner.ProviderID = request.ProviderID;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

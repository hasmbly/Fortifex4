using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Owners.Commands.UpdateExchangeOwner;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Owners.Commands.UpdateExchangeOwner
{
    public class UpdateExchangeOwnerCommandHandler : IRequestHandler<UpdateExchangeOwnerRequest, UpdateExchangeOwnerResponse>
    {
        private readonly IFortifex4DBContext _context;

        public UpdateExchangeOwnerCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<UpdateExchangeOwnerResponse> Handle(UpdateExchangeOwnerRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateExchangeOwnerResponse();

            var owner = await _context.Owners
                .Where(x => x.OwnerID == request.OwnerID)
                .SingleOrDefaultAsync(cancellationToken);

            if (owner == null)
            {
                result.IsSucessful = false;
                result.ErrorMeesage = ErrorMessage.OwnerNotFound;
            }
            
            owner.ProviderID = request.ProviderID;

            await _context.SaveChangesAsync(cancellationToken);

            result.IsSucessful = true;

            return result;
        }
    }
}
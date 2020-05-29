using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Owners.Commands.CreateExchangeOwner;
using MediatR;

namespace Fortifex4.Application.Owners.Commands.CreateExchangeOwner
{
    public class CreateExchangeOwnerCommandHandler : IRequestHandler<CreateExchangeOwnerRequest, CreateExchangeOwnerResponse>
    {
        private readonly IFortifex4DBContext _context;

        public CreateExchangeOwnerCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<CreateExchangeOwnerResponse> Handle(CreateExchangeOwnerRequest request, CancellationToken cancellationToken)
        {
            if (_context.Owners.Any(x => x.MemberUsername == request.MemberUsername && x.ProviderID == request.ProviderID))
                throw new ArgumentException($"Owner with username {request.MemberUsername} and ProviderID {request.ProviderID} already exists");

            var entity = new Owner
            {
                MemberUsername = request.MemberUsername,
                ProviderID = request.ProviderID,
                ProviderType = ProviderType.Exchange
            };

            _context.Owners.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateExchangeOwnerResponse 
            {
                IsSucessful = true,
                OwnerID = entity.OwnerID
            };
        }
    }
}
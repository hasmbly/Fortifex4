using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Application.Owners.Commands.CreateExchangeOwner
{
    public class CreateExchangeOwnerCommand : IRequest<int>
    {
        public string MemberUsername { get; set; }
        public int ProviderID { get; set; }
    }

    public class CreateExchangeOwnerCommandHandler : IRequestHandler<CreateExchangeOwnerCommand, int>
    {
        private readonly IFortifex4DBContext _context;

        public CreateExchangeOwnerCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateExchangeOwnerCommand request, CancellationToken cancellationToken)
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

            return entity.OwnerID;
        }
    }
}
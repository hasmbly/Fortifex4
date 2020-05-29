using MediatR;

namespace Fortifex4.Shared.Owners.Commands.CreateExchangeOwner
{
    public class CreateExchangeOwnerRequest : IRequest<CreateExchangeOwnerResponse>
    {
        public string MemberUsername { get; set; }
        public int ProviderID { get; set; }
    }
}
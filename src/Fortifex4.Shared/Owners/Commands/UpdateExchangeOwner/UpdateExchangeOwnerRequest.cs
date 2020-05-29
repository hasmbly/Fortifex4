using MediatR;

namespace Fortifex4.Shared.Owners.Commands.UpdateExchangeOwner
{
    public class UpdateExchangeOwnerRequest : IRequest<UpdateExchangeOwnerResponse>
    {
        public int OwnerID { get; set; }
        public int ProviderID { get; set; }
    }
}
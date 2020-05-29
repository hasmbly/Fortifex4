using MediatR;

namespace Fortifex4.Shared.Owners.Commands.DeleteOwner
{
    public class DeleteOwnerRequest : IRequest<DeleteOwnerResponse>
    {
        public int OwnerID { get; set; }
    }
}